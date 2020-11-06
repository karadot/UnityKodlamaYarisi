using UnityEngine;

public class KontrolEdilebilirHareketli : Hareketli {
    //Her hareketli için farklı input axisleri tanımlayabilmemizi sağlayan değişkenimiz.
    [SerializeField]
    string yonEkseni = "Vertical";

    public override void HareketEt () {
        float input = Input.GetAxis (yonEkseni);
        //Yalnızca istediğimiz axis üzerinden 0 dışında bir değer alabildiysek hareketi sağlıyoruz.
        if (input != 0) {
            /*
            Burada input değerini kullanmamın tek sebebi yumuşatmadan yararlanmak. Eğer input
            değişkeni ile çarpmazsanız anında ve sabit hızlı bir hareket oluşacaktır.
            */
            Vector3 hedefKonum = transform.position + new Vector3 (Yon.x, Yon.y, 0) * input;
            SinirlariKontrolEt (ref hedefKonum);
            transform.position = hedefKonum;
        }
    }

    //Sınır atamalarını yapıyoruz.
    public override void Sinirlar (Vector2 solAlt, Vector2 sagUst) {
        Alt = solAlt.y;
        Ust = sagUst.y;
        Sol = solAlt.x;
        Sag = sagUst.x;
    }

    /*
    Referans olarak verilen konum sınırları geçiyorsa, konumu gerekli değerlerle güncelliyoruz.
    Bu sayede sınır içerisinde sabit kalıyor.
    */
    public override void SinirlariKontrolEt (ref Vector3 konum) {
        if (konum.x > Sag)
            konum.x = Sag;
        if (konum.x < Sol)
            konum.x = Sol;
        if (konum.y > Ust)
            konum.y = Ust;
        if (konum.y < Alt)
            konum.y = Alt;
    }
}
