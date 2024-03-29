﻿using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Levels.presentation.ui
{
    public class LevelsList : MonoBehaviour
    {
        [SerializeField] private Transform listRoot;
        [Inject] private LevelItem.Factory levelItemFactory;
        [Inject] private ILevelsRepository levelsRepository;

        private void Awake()
        {
            if (listRoot == null)
                listRoot = transform;

            levelsRepository.GetLevels().ForEach(CreateItem);
        }

        private void CreateItem(Level level)
        {
            var item = levelItemFactory.Create();
            item.transform.SetParent(listRoot);
            item.Setup(level);
        }
    }
}