﻿using Orders.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities
{
    public class Country : IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "País")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo{0} es requerido.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Estados/Departamentos")]
        public int StatesNumber => States == null || States.Count == 0 ? 0 : States.Count;

        public ICollection<State>? States { get; set; }
    }
}