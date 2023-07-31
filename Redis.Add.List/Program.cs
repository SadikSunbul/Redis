
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;

public class Kitap
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Yazar { get; set; }
    public int SayfaSayisi { get; set; }
}

public class RedisRepository
{
    private readonly ConnectionMultiplexer redisConnection;

    public RedisRepository(string connectionString)
    {
        redisConnection = ConnectionMultiplexer.Connect(connectionString);
    }

    public void KitapListesiniRedisEkle(List<Kitap> kitaplar)
    {
        var database = redisConnection.GetDatabase();
        foreach (var kitap in kitaplar)
        {
            var hashKey = "kitap:" + kitap.Id;

            // Kitap verilerini Redis Hash türünde ekleyin
            var hashEntries = new HashEntry[]
            {
                new HashEntry("Ad", kitap.Ad),
                new HashEntry("Yazar", kitap.Yazar),
                new HashEntry("SayfaSayisi", kitap.SayfaSayisi)
            };
            
            database.HashSet(hashKey, hashEntries);
        }
    }
}



class Program
{
    static void Main()
    {
        var kitaplar = new List<Kitap>
        {
            new Kitap { Id = 1, Ad = "Kitap 1", Yazar = "Yazar 1", SayfaSayisi = 200 },
            new Kitap { Id = 2, Ad = "Kitap 2", Yazar = "Yazar 2", SayfaSayisi = 300 },
            new Kitap { Id = 3, Ad = "Kitap 3", Yazar = "Yazar 3", SayfaSayisi = 250 }
        };

        var redisConnectionString = "localhost:1452"; // Redis sunucu bağlantı bilgisi
        var redisRepository = new RedisRepository(redisConnectionString);
        redisRepository.KitapListesiniRedisEkle(kitaplar);

        Console.WriteLine("Kitaplar Redis'e başarıyla eklendi!");
    }
}
