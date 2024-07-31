using BlazingBooks.Web.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

public class GenreBooks
{
    public short GenreId { get; set; }

    public int BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public virtual Book Book { get; set; }

    [ForeignKey(nameof(GenreId))]
    public virtual Genre Genre { get; set; }
}