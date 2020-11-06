using UnityEngine;
using UnityEngine.UI;

public class PongKontrol : MonoBehaviour {
    //Kontrol etmek istediğimiz objeleri atıyoruz, bu durumda sağ ve sol kollar oluyor.
    [SerializeField]
    Hareketli sagKol, solKol;
    //top objemiz
    [SerializeField]
    SabitHareketli top;
    //Bu iki değişkene oyun sınırlarını atıyoruz
    Vector3 solAlt, sagUst;
    //skorları yazdırmak için kullanacağımız Text bileşenleri
    [SerializeField]
    Text sagText, solText;
    int sagSkor, solSkor;

    void Start () {
        /*
        Kamera bileşeni üzerinden ekranın sağ üst ve sol alt köşelerinin
        gerçek x,y konumlarını alıyorum. Z konumu bu durumda ihtiyacım olan bir şey değil.
        Burada sağlıklı olarak alabilmemizin nedeni de kamera tipinin Orthgraphic olması. Aklınızda bulunsun.
        */
        solAlt = GetComponent<Camera> ().ViewportToWorldPoint (Vector2.zero);
        sagUst = GetComponent<Camera> ().ViewportToWorldPoint (Vector2.one);

        /*
        Videodan farklı olarak ufak bir ekleme daha yaptım. 
        Bu sayede kolları sağ ve sol sınıra tam olarak yerleştirebiliyorum.
        Ufak bir hatırlatma olsun, yukarı aldığımız sınır değişkenleri 
        aslında x için sağ-sol değerleri y içinde de yukarı-aşağı değerleri tutuyoruz.
        Bu yüzden burada x değerlerini kullandım.
        solAlt=>minX,minY
        sağUst=>maxX,maxY
        */
        sagKol.transform.position = new Vector3 (sagUst.x, 0, 0);
        solKol.transform.position = new Vector3 (solAlt.x, 0, 0);

        sagKol.Sinirlar (solAlt, sagUst);
        solKol.Sinirlar (solAlt, sagUst);
        top.Sinirlar (solAlt, sagUst);
        /*
        Kol objeleri için yön değişkenini atıyoruz.
        Sadece dikeyde hareket etmelerini istediğim için vector2.up değerini veriyorum.
        Farklı hareket yönleri vererek nasıl bir etki oluşacağını denyebilirsiniz.
        */
        sagKol.Yon = Vector2.up;
        solKol.Yon = Vector2.up;

        /*
        Topumuza rastgele yön verdikten sonra skorEvent ile
        skor olma durumunu dinlemeye başlıyoruz. Eğer skorEvent çağırılırsa
        Skor isimli fonksiyonumuz da çağırılacak..
        */
        top.RastgeleYon ();
        top.skorEvent.AddListener (Skor);
    }
    void Update () {
        /*
        Update içerisinde bu objelerimizin hareketini sağlıyoruz.
        Kulağa gereksiz geliyor olabilir, ancak bu sisteme bir 
        oyunu durdur,devam et sistemi eklemek için çok ideal. 
        Space tuşuna basıldığında oyunu durdurup devam ettirecek bir yapıyı
        size egzersiz olarak bırakıyorum.
        */
        sagKol.HareketEt ();
        solKol.HareketEt ();
        top.HareketEt ();
    }

    /*
    Skor olma durumuna göre top sağda ise solkol, solda ise sağkol skor yapmıştır.
    Buna göre skor değerlerini arttırıp gerekli Text bileşenlerini güncelliyoruz.
    */
    void Skor () {
        if (top.transform.position.x > 0) {
            solSkor++;
            solText.text = solSkor.ToString ();
        } else {
            sagSkor++;
            sagText.text = sagSkor.ToString ();
        }
    }
}
