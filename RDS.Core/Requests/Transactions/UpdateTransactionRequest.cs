using System.ComponentModel.DataAnnotations;
using RDS.Core.Common.Extensions;
using RDS.Core.Enums;

namespace RDS.Core.Requests.Transactions;

public abstract class UpdateTransactionRequest : Request
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "Título inválido")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo inválido")]
    public ETransactionType Type { get; set; }

    [Required(ErrorMessage = "Valor inválido")]
    [GreaterThanZero(ErrorMessage = "O valor deve ser maior do que zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Categoria inválida")]
    public long CategoryId { get; set; }

    [Required(ErrorMessage = "Data inválida")]
    public DateTime? PaidOrReceivedAt { get; set; }
}