using System;
using System.Collections.Generic;
using Services.Gameplay.Units;
using UnityEngine;

namespace Controllers.PlayerControls
{
    public class UnitSelectionManager : IDisposable
    {
        public IReadOnlyCollection<Unit> selectedUnits => _selectedUnits;
        private readonly HashSet<Unit> _selectedUnits = new();

        public UnitSelectionManager()
        {
        }

        public void SelectUnit(Unit unit)
        {
            _selectedUnits.Add(unit);
            unit.SetSelected();

            Debug.Log($"Unit {unit.name} selected. Count - {_selectedUnits.Count}");
        }

        public void DeselectUnit(Unit unit)
        {
            unit.SetUnselected();
            _selectedUnits.Remove(unit);
        }

        public void ClearSelection()
        {
            Debug.Log($"Units deselected");

            foreach (var selectedUnit in _selectedUnits)
            {
                selectedUnit.SetUnselected();
            }

            _selectedUnits.Clear();
        }

        public void Dispose()
        {
        }
    }
}