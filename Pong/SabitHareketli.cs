using UnityEngine;
using UnityEngine.Events;
/*
Verilen sınırlar içerisinde sahip olduğu yön değerine göre hareket eden
bir obje elde etmemizi sağlar. Amacımız pong oyunu olduğu için
burada sadece yukarı aşağı sınırlarında ve başka bir objeye çarptığında 
yön değerini değiştiriyoruz.
Sağ ve sol ile skor yapacağı için, rastgele yön atayıp 0,0 noktasına geri alıyoruz sadece.
Ayrıca sağ-sol sınırları geçmesi durumunda tetiklediğimiz bir event içeriyor.
*/
public class SabitHareketli : Hareketli {

    //Topun skor durumunu bildirmek için kullandığımız event
    public UnityEvent skorEvent;

    public override void HareketEt () {
        /*
        Çarpışma kontrolünü başta yapıyor olmamızın nedeni
        hareket gerçekleşmeden önce gidilecek konumda bir engel var mı yok mu
        buna göre hareketi belirlemek.
        */
        CarpismaKontrol ();
        Vector3 hedefKonum = transform.position + new Vector3 (Yon.x, Yon.y, 0);
        SinirlariKontrolEt (ref hedefKonum);
        transform.position = hedefKonum;
    }

    void CarpismaKontrol () {
        Vector3 rayYon;

        //Ray yönümüzü topun ekranda sağa ya da sola gitmesine göre belirliyoruz.

        if (Yon.x > 0) {
            rayYon = Vector3.right;
        } else {
            rayYon = Vector3.left;
        }

        /*
        Burada kullandığımız 1f değeri, aslında top objemizin scale değeri ile aynı.
        Şayet daha büyük bir obje kullanırsanız, çarpışmalarda içiçe geçme gibi
        durumlarla karşılaşabilirsiniz.
        */
        RaycastHit2D hits = Physics2D.Raycast (transform.position, rayYon, 1f);
        if (hits) {
            float aci = 0;
            /*
            Videoda göreceğiniz gibi bu kısmı anlatırken biraz zorlandım.
            Ancak basitçe, şayet sağ tarafta kontrolcüye çarparsa
            ve kontrolcüden daha aşağı bir noktadaysa Sol Aşağı yöne gitmesini istiyorum
            Unity'de sağ taraf 0 derece olarak kabul edildiği için değerleri de buna göre belirledim.
            Sağdan kastım şu; karşımızda bir x-y eksenlerinden oluşan bir alan düşünürsek, 
            0 derece +x , 90 derece +y 
            180 derece -x ve son olarak 270 derece de -y yönüne bakıyor olacaktır.
            Bu bilgiler ışığında sol aşağı,sol yukarı, sağ aşağı, sağ yukarı şeklinde açıları belirledim.
            */
            if (transform.position.x > 0) { //eğer top sağ tarafı geçtiyse
                if (hits.transform.position.y > transform.position.y) {
                    aci = 225; //sol aşağı
                } else {
                    aci = 135; //sol yukarı
                }
            } else { //eğer top sol tarafı geçtiyse
                if (hits.transform.position.y > transform.position.y) {
                    aci = -45; //sağ aşağı
                } else {
                    aci = 45; //sağ yukarı
                }
            }
            /*
            Ufak bir hatırlatma. Bu yaptığımı çok basit bir şey ve oynaması da pek zevkli olmuyor açıkçası.
            Bunun yerine ister matematik bilginizi kullanarak isterseniz de internette araştırmalar yaparak
            daha gerçekçi bir çarpışma ve yön değiştirme etkisi elde edebilirsiniz, sizin için ufak bir egzersiz olarak bırakıyorum.
            Tüyo olarak da söyleyeceğim tek şey: Pong Hit Direction
            */

            /*
            Burada önemli olan kısım Mathf.Deg2Rad ile derece olan değerlerimizi radyan cinsine çevirmek
            Çünkü Mathf.Cos ve Sin fonksiyonları radyan değeri olarak kabul edecektir, bunu yapmazsak
            istemediğimiz sonuçlar çıkması olası. Tabii ki derece yerine radyan olarak da hesaplayabilirsiniz.
            Eğer radyan cinsinden bir değere ihtiyacınız varsa Deg2Rad işlemine gerek kalmayacaktır. 
            */
            Yon = new Vector2 (Mathf.Cos (aci * Mathf.Deg2Rad), Mathf.Sin (aci * Mathf.Deg2Rad));
        }
    }

    //Sinirlari için değişkenlerimizi oluşturuyoruz. Bu fonksiyonu çağırmazsanız 0,0 noktasında sabit kalacaktır
    public override void Sinirlar (Vector2 solAlt, Vector2 sagUst) {
        Alt = solAlt.y;
        Ust = sagUst.y;
        Sol = solAlt.x;
        Sag = sagUst.x;
    }

    /*
    Gideceğimiz konumun sınırları geçip geçmediğini kontrol ediyoruz
    Ref komutu sayesinde değişkeni referans olarak alıyoruz, yani üzerinde direkt
    değişiklikler yaparak güncelleyebiliyoruz.
    */
    public override void SinirlariKontrolEt (ref Vector3 konum) {
        /*
        X değerimiz sag-sol sınırlarımızdan birini geçtiyse skor olmuştur.
        Burada hedef konumdan varolan poziyon değerini çıkarıyorum. Ancak bu
        tamamen hareket kodumuzla alakalı. Bu kod içerisinde hareketi
        direkt pozisyonu atayarak yaptığımız için, aslında vereceğimiz pozisyonu güncellemiş oluyoruz.
        Bunun yerine Vector3.zero ataması da yapabilirsiniz, direkt 0,0 noktasından başlamasını sağlarsınız böylece.
        Ve tabii ki skorEvent çağırıyoruz, bu sayede bu event'ı dinleyen başka sınıflar varsa, ki bu durumda bizim oyun 
        kontrolcümüz dinliyor, bu olaydan haber olacaktır. 
        Son olarak da yön değerini rastgele hale getiriyoruz.
        */
        if (konum.x > Sag || konum.x < Sol) {
            konum -= transform.position;
            skorEvent?.Invoke ();
            RastgeleYon ();
        }
        /*
        Burada üst ve alt sınırlar için kontrol yapıyoruz. Geçtiği sınırın tersi yönde hareket alması için yalnızca y eksenini
        negatif hale getiriyoruz.
        */
        if (konum.y > Ust || konum.y < Alt) {
            Yon = new Vector2 (Yon.x, -Yon.y);
        }
    }
    /*
    Çok temel ve basit düzeyde bir random yön belirleme fonksiyonu oldu bu.
    Sadece 0,0 değerleri almasını ve direkt olarak yukarı veya aşağı yön değerlerine
    sahip olmasını engelliyoruz.
    Kesinlikle çok daha iyi bir yapı kurulabilir ve daha iyi sonuçlar elde edilebilir.
    */
    public void RastgeleYon () {
        float x, y;
        do {
            x = Random.Range (-1, 1);
            y = Random.Range (-1, 1);
        } while (y == -1 || y == 1 || (y == 0 && x == 0));

        Yon = new Vector2 (x, y);
    }
}
