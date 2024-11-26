using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        
        if (await session.Query<Product>().AnyAsync())
            return;

        session.Store<Product>(GetPreconfigureProducts());
        await session.SaveChangesAsync();  
    }

    private static IEnumerable<Product> GetPreconfigureProducts() => new List<Product>
    {
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Camiseta Básica",
            Description = "Camiseta de algodón 100% para uso diario.",
            Category = new List<string>{ "Ropa", "Camisetas" },
            ImageUrl = "https://example.com/images/camiseta-basica.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Pantalón Deportivo",
            Description = "Pantalón cómodo y flexible para hacer ejercicio.",
            Category = new List<string>{ "Ropa", "Pantalones", "Deportivo" },
            ImageUrl = "https://example.com/images/pantalon-deportivo.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Zapatillas Running",
            Description = "Zapatillas diseñadas para running con gran amortiguación.",
            Category = new List<string>{ "Calzado", "Deportivo" },
            ImageUrl = "https://example.com/images/zapatillas-running.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Chaqueta Impermeable",
            Description = "Chaqueta impermeable ideal para condiciones de lluvia.",
            Category = new List<string>{ "Ropa", "Chaquetas", "Outdoor" },
            ImageUrl = "https://example.com/images/chaqueta-impermeable.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Gorra Deportiva",
            Description = "Gorra ligera para protegerse del sol mientras haces ejercicio.",
            Category = new List<string>{ "Accesorios", "Deporte" },
            ImageUrl = "https://example.com/images/gorra-deportiva.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Reloj Inteligente",
            Description = "Reloj inteligente con monitorización de actividad y frecuencia cardíaca.",
            Category = new List<string>{ "Tecnología", "Accesorios", "Relojes" },
            ImageUrl = "https://example.com/images/reloj-inteligente.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Auriculares Bluetooth",
            Description = "Auriculares con conexión Bluetooth para escuchar música sin cables.",
            Category = new List<string>{ "Tecnología", "Audio" },
            ImageUrl = "https://example.com/images/auriculares-bluetooth.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Mochila para Laptop",
            Description = "Mochila resistente con compartimento especial para laptop.",
            Category = new List<string>{ "Accesorios", "Bolsos", "Tecnología" },
            ImageUrl = "https://example.com/images/mochila-laptop.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Silla Ergonómica",
            Description = "Silla de oficina ergonómica para largas jornadas de trabajo.",
            Category = new List<string>{ "Mobiliario", "Oficina" },
            ImageUrl = "https://example.com/images/silla-ergonomica.jpg"
        },
        new Product()
        {
            Id = Guid.NewGuid(),
            Name = "Lámpara LED",
            Description = "Lámpara de escritorio LED con ajuste de brillo.",
            Category = new List<string>{ "Mobiliario", "Iluminación" },
            ImageUrl = "https://example.com/images/lámpara-led.jpg"
        }
    };
}
