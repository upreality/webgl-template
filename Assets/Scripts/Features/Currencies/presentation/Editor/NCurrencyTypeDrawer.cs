using System;
using System.Collections.Generic;
using System.Linq;
using Features.Currencies.data;
using Features.Currencies.domain.model;
using UnityEditor;
using UnityEngine;
using Utils.Editor;

namespace Features.Currencies.presentation.Editor
{
    [CustomPropertyDrawer(typeof(NCurrencyType))]
    public class NCurrencyTypeDrawer : PropertyDrawer
    {
        int choiceIndex;

        private Lazy<List<Currency>> currencies = new(GetCurrencies);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var currencyIDProperty = property.FindPropertyRelative("ID");

            EditorGUI.BeginChangeCheck();
            var choices = currencies.Value.Select(currency => currency.Name).ToArray();
            var currentIndex = FindCurrencyIndex(property);
            choiceIndex = EditorGUI.Popup(position, currentIndex, choices);
            
            if (!EditorGUI.EndChangeCheck()) 
                return;
            
            currencyIDProperty.stringValue = currencies.Value[choiceIndex].ID;
        }

        private int FindCurrencyIndex(SerializedProperty property)
        {
            var currentId = property.FindPropertyRelative("ID").stringValue;
            return currencies.Value.FindIndex(currency => currency.ID == currentId);
        }

        private static List<Currency> GetCurrencies() => FindRepository(out var currencyRepository)
            ? currencyRepository.GetCurrencies()
            : new List<Currency>();

        private static bool FindRepository(out SOCurrencyRepository repository)
        {
            repository = null;
            var repositories = SOEditorUtils.GetAllInstances<SOCurrencyRepository>();
            if (!repositories.Any()) return false;
            repository = repositories.First();
            return true;
        }
    }
}