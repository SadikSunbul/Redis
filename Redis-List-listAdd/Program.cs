using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
            var listKey = "kitap_listesi";

            // Kitap verilerini JSON formatında serileştirin
            var kitapJson = JsonConvert.SerializeObject(kitap);

            // Redis List'e ekleyin
            database.ListRightPush(listKey, kitapJson);
        }
    }

    public List<Kitap> KitapListesiniRedisOku()
    {
        var database = redisConnection.GetDatabase();
        var listKey = "kitap_listesi";

        // Redis List'teki tüm öğeleri okuyun
        var kitaplarJson = database.ListRange(listKey);

        // JSON'dan Kitap nesnesine dönüştürün
        var kitaplar = new List<Kitap>();
        foreach (var kitapJson in kitaplarJson)
        {
            var kitap = JsonConvert.DeserializeObject<Kitap>(kitapJson);
            kitaplar.Add(kitap);
        }

        return kitaplar;
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

        // Kitapları Redis'e eklemek için
        redisRepository.KitapListesiniRedisEkle(kitaplar);
        Console.WriteLine("Kitaplar Redis List'e başarıyla eklendi!");

        // Kitapları Redis List'ten okumak için
        var kitaplarOkunan = redisRepository.KitapListesiniRedisOku();
        foreach (var kitap in kitaplarOkunan)
        {
            Console.WriteLine($"Id: {kitap.Id}, Ad: {kitap.Ad}, Yazar: {kitap.Yazar}, Sayfa Sayısı: {kitap.SayfaSayisi}");
        }
    }
}
