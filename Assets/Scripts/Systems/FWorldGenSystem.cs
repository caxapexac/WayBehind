using System;
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
		Forest
	}

	public enum ForegroundTypes
	{
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
		public GameObject Player;
		public Sprite Hexagon;
		public Sprite Obstacle;
		public Sprite Water;
		public Sprite Grass;
		public Sprite Boloto;

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
			RenderFull(new CubeCoords(0, 0), Fow + 1);
		}

		public void Run()
		{
			CubeCoords playerCoords = HexMath.Pix2Hex(_playerFilter.Components1[0].Transform.localPosition.x,
				                                      _playerFilter.Components1[0].Transform.localPosition.y, HexSize);
			if (HexMath.HexDistance(lastCoords.x, playerCoords.x, lastCoords.y, playerCoords.y) > Fow / 2)
			{
				RenderNext(playerCoords, Fow);
				lastCoords = playerCoords;
				//UnrenderRing(playerCoords, Fow + 2);
				//RenderNext(playerCoords, Fow + 1);
			}
		}

		private void RenderFull(CubeCoords playerCoords, int radius)
		{
			RenderHexBackground(playerCoords);
			RenderHexForeground(playerCoords);
			for (int i = 0; i < radius; i++)
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
					HideHex(coords);
				}
			}
		}

		private void RenderNext(CubeCoords playerCoords, int radius)
		{
			CubeCoords coords = new CubeCoords(playerCoords.x, playerCoords.y + radius);
			CubeCoords difference = new CubeCoords(playerCoords.x - lastCoords.x, playerCoords.y - lastCoords.y);
			if (difference.x == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < radius; j++)
					{
						coords.x += HexMath.Directions[i, 0];
						coords.y += HexMath.Directions[i, 1];
						if (Mathf.Sign(coords.y) == Mathf.Sign(difference.y) 
						    || Mathf.Sign(coords.z) == Mathf.Sign(difference.z))
						{
							RenderHexBackground(coords);
							RenderHexForeground(coords);
						}
						else
						{
							//HideHex(coords);
						}
					}
				}
			}
			else if (difference.y == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < radius; j++)
					{
						coords.x += HexMath.Directions[i, 0];
						coords.y += HexMath.Directions[i, 1];
					    if (Mathf.Sign(coords.x) == Mathf.Sign(difference.x) 
					        || Mathf.Sign(coords.z) == Mathf.Sign(difference.z))
						{
							RenderHexBackground(coords);
							RenderHexForeground(coords);
						}
					    else
					    {
						    //HideHex(coords);
					    }
					}
				}
			}
			else
			{
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < radius; j++)
					{
						coords.x += HexMath.Directions[i, 0];
						coords.y += HexMath.Directions[i, 1];
						if (Mathf.Sign(coords.x) == Mathf.Sign(difference.x) 
						    || Mathf.Sign(coords.y) == Mathf.Sign(difference.y))
						{
							RenderHexBackground(coords);
							RenderHexForeground(coords);
						}
						else
						{
							//HideHex(coords);
						}
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
			//if (hexComponent.Parent != null) _poolBackground.Recycle(hexComponent.Parent);
			hexComponent.Parent = _poolBackground.Get();
			hexComponent.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
			switch (hexComponent.GroundType)
			{
				case BackroundTypes.Grass:
					hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Grass;
					break;
				case BackroundTypes.Water:
					hexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Water;
					break;
				case BackroundTypes.Forest:
					//todo
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
			//if (foregroundHexComponent.Parent != null) _poolForeground.Recycle(foregroundHexComponent.Parent);
			foregroundHexComponent.Parent = _poolForeground.Get();
			foregroundHexComponent.Parent.PoolTransform.localPosition = HexMath.Hex2Pix(coords, HexSize);
			switch (foregroundHexComponent.ObjectType)
			{
				case ForegroundTypes.Enemy:
					//todo
					break;
				case ForegroundTypes.Obstacle:
					foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = Obstacle;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = false;
					break;
				case ForegroundTypes.Diamond:
					//todo
					break;
				case ForegroundTypes.Player:
					foregroundHexComponent.Parent.PoolTransform.GetComponent<SpriteRenderer>().sprite = null;
					foregroundHexComponent.Parent.PoolTransform.GetComponent<Collider2D>().isTrigger = true;
					break;
				default:
					throw new Exception("Null object type");
			}
			foregroundHexComponent.Parent.PoolTransform.gameObject.SetActive(true);
		}

		public void HideHex(CubeCoords coords)
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
