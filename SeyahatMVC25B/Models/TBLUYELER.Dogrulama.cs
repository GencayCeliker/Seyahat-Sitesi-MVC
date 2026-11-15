using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeyahatMVC25B.Models
{
    // TBLUYELER.cs Entity Framework tarafından üretilir ve model güncellenince üzerine yazılır.
    // Üye Ol formundaki "şifre tekrar" alanı bu yüzden ayrı bir partial sınıfta tutulur.
    public partial class TBLUYELER
    {
        [NotMapped]
        [Compare("SIFRE", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string SIFRETEKRAR { get; set; }
    }
}
