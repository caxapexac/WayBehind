using System;
using System.Runtime.Remoting.Messaging;
using Components;
using Leopotam.Ecs;
using LeopotamGroup.Collections;
using LeopotamGroup.Math;
using LeopotamGroup.Pooling;
using Misc;
using TMPro;
using UnityEngine;

namespace Systems
{
	public enum BackroundTypes
	{
		Grass,
		Water,
		Forest,
		Boloto
	}

	public enum ForegroundTypes
	{
		Empty,
		Player,
		Enemy,
		Diamond,
		Obstacle
	}
	
	[EcsInject]
	public class FWorldGenSystem : IEcsInitSystem, IEcsRunSystem
	{
		public float HexSize;
		public int Fow;
		public Sprite Player;
		public Sprite Enemy;
		public Sprite Diamond;
		public Sprite Obstacle;
		public Sprite Water;
		public Sprite Grass;
		public Sprite Boloto;
		public Sprite Forest;

		private CubeCoords lastCoords;
		
		private PoolContainer _poolBackground;
		private PoolContainer _poolForeground;

		private HexList4D<HexBackgroundComponent> _backgroundMap;
		private HexList4D<HexForegroundComponent> _foregroundMap;

		private EcsFilter<PlayerComponent> _playerFilter = null;
		
		public void Initialize()
		{
			lastCoords = new CubeCoords(0, 0);
			_poolBackground = PoolContainer.CreatePool(Utils.BackPrefabPath);
			_poolForeground = PoolContainer.CreatePool(Utils.ForePrefabPath);
			_backgroundMap = MapGenRandomNeighbours.GenerateBackground(64);
			_foregroundMap = MapGenRandomNeighbours.GenerateForeground(_backgroundMap);
			RenderFull(new CubeCoords(0, 0), Fow);
		}

		public void Run()
		{
			CubeCoords playerCoords = HexMath.Pix2Hex(_playerFilter.Components1[0].Transform.localPosition.x,
				                                      _playerFilter.Components1[0].Transform.localPosition.y, HexSize);
			if (lastCoords.x != playerCoords.x || lastCoords.y != playerCoords.y)
			{
				UnrenderRing(lastCoords, Fow+1);
			    RenderRing(playerCoords, Fow);
                lastCoords = playerCoords;
			}
		}

		private void RenderFull(CubeCoords playerCoords, int radius)
		{
			RenderHexBackground(playerCoords);
			RenderHexForeground(playerCoords);
			for (int i = 1; i <= radius; i++)
			{
				RenderRing(playerCoords, i);
			}
		}

		private void RenderRing(CubeCoords playerCoords, int radius)
		{
			CubeCoords coords = new CubeCoords(playerCoords.x, playerCoords.y + radius);
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					coords.x += HexMath.Directions[i, 0];
					coords.y += HexMath.Directions[i, 1];
					RenderHexBackground(coords);
					RenderHexForeground(coords);
				}
			}
		}

		private void UnrenderRing(CubeCoords playerCoords, int radius)
		{
			CubeCoords coords = new CubeCoords(playerCoords.x, playerCoords.y + radius);
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					coords.x += HexMath.Directions[i, 0];
					coords.y += HexMath.Directions[i, 1];
					if (HexMath.HexDistance(playerCoords.x, playerCoords.y, coords.x, coords.y) >= Fow)
					{
						HideHex(coords);
					}
				}
			}
		}

		public void RenderHexBackground(CubeCoords coords)
		{
			if (!_backgroundMap.ExistAt(coords) || _backgroundMap[coords.x, coords.y].IsNew)
			{
				MapGenRandomNeighbours.GenerateHex(coords, _backgroundMap, _foregroundMap);
			}
			HexBackgroundComponent hexComponent = _backgroundMap[coords.x, coords.y];
			if (hexComponent.Parent != null) return; //_poolBackground.Recycle(hexComponent.Parent);
			hexComponent.Parent = _poolBackground.Get();
			hexComponent.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
			switch (hexComponent.GroundType)
			{
				case BackroundTypes.Grass:
					hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Grass;
					hexComponent.SpeedDown = 1;
					break;
				case BackroundTypes.Water:
					hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Water;
					hexComponent.SpeedDown = 0.1f;
					break;
				case BackroundTypes.Boloto:
					hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Boloto;
					hexComponent.SpeedDown = 0.02f;
					break;
				case BackroundTypes.Forest:
					hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Forest;
					hexComponent.SpeedDown = 0.5f;
					break;
				default:
					throw new Exception("Null ground type");
			}
			hexComponent.Parent.PoolTransform.gameObject.SetActive(true);
		}
		
		private void RenderHexForeground(CubeCoords coords)
		{
			if (!_foregroundMap.ExistAt(coords) || _foregroundMap[coords.x, coords.y].IsNew) return;
		    HexForegroundComponent foregroundHexComponent = _foregroundMap[coords.x, coords.y];
			if (foregroundHexComponent.ObjectType == ForegroundTypes.Empty) return;
			if (foregroundHexComponent.Parent != null) return; //_poolForeground.Recycle(foregroundHexComponent.Parent);
			foregroundHexComponent.Parent = _poolForeground.Get();
			foregroundHexComponent.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
			switch (foregroundHexComponent.ObjectType)
			{
				case ForegroundTypes.Empty:
					return;
				case ForegroundTypes.Enemy:
					foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Enemy;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = true;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = false;
					break;
				case ForegroundTypes.Obstacle:
					foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Obstacle;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = true;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = false;
					break;
				case ForegroundTypes.Diamond:
					foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Diamond;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = true;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = true;
					break;
				case ForegroundTypes.Player:
					//spawn point, dunno why
					foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = null;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().enabled = false;
					break;
				default:
					throw new Exception("Null object type");
			}
			
			foregroundHexComponent.Parent.PoolTransform.gameObject.SetActive(true);
		}

		private void HideHex(CubeCoords coords)
		{
			if (!_backgroundMap.ExistAt(coords)) return;
			HexBackgroundComponent hexComponent = _backgroundMap[coords.x, coords.y];
			if (hexComponent.Parent == null) return;
			_poolBackground.Recycle(hexComponent.Parent);
			hexComponent.Parent = null;
			if (!_foregroundMap.ExistAt(coords)) return;
			HexForegroundComponent foregroundHexComponent = _foregroundMap[coords.x, coords.y];
			if (foregroundHexComponent.Parent == null) return;
		    _poolForeground.Recycle(foregroundHexComponent.Parent);
			foregroundHexComponent.Parent = null;
		}
		
		public void Destroy()
		{
		
		}
	
	}
}
