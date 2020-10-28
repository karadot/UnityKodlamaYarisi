using System.Collections.Generic;
using UnityEngine;
/* Video başında aldığım notlar:
-baş kısmının yönünü belirle
-yöne göre hareket ettir
-pozisyonlar listesi, 0.elamanı
-kuyruklar listesi, pozisyonlar-1
-enum Yonler
*/

public class YilanOyunu : MonoBehaviour {
    //Yem yedikçe yılana ekleyeceğim kuyruk için prefab
    [SerializeField]
    GameObject kuyrukPrefab;
    /*
    Kuyruk parçalarını ve konumları listelerde tutup hareket ettiriyoruz.
    Burada temel mantık şu:
    Konum 0=Baş obje olacak. Başlangıçta 0 adet kuyruk 1 adet pozisyon var.
    Örneğin 1 adet kuyruk ekledik, bu kuyruk 0. kuyruk olacak
    ve listedeki 0.pozisyonu alacak her harekete. Tabii her harekette
    biz de pozisyonlara
    hem bu kuyruğu hem de baş kısmın pozisyonunu ekleyeceğiz.
    Böylece ilk kuyruk parçaası başı, ikinci kuyruk ilk kuyruğu, 
    üçüncü kuyruk ikinci kuyruğu ... takip edecek
    şekilde ilerlemeyi sağlayacağız.
    */
    List<Vector3> konumlar = new List<Vector3> ();
    List<Transform> kuyruk = new List<Transform> ();

    //Yılanın gideceği yön verisi
    Yonler yon = Yonler.sag;

    int timer = 0;
    /*
    Kaç karede bir hareket işlemini gerçekleştirdiğimiz belirlediğimiz değişken.
    Bu haliyle 6 defa update çağırıldıktan sonra işlem yapıyoruz. 
    */
    int hareketSuresi = 6;

    bool yemYedi = false;
    // Start is called before the first frame update
    void Start () {
        konumlar.Add (transform.position);
    }

    // Update is called once per frame
    void Update () {
        timer++;

        //Bu if blogları ile kullanıcıdan girdi alıp, ters yöne gitmeye çalışmadığı durumlarda yön değiştiriyoruz.
        if (Input.GetKeyDown (KeyCode.W)) {
            if (!yon.ZitMi (Yonler.yukari))
                yon = Yonler.yukari;
        }

        if (Input.GetKeyDown (KeyCode.S)) {
            if (!yon.ZitMi (Yonler.asagi))
                yon = Yonler.asagi;
        }

        if (Input.GetKeyDown (KeyCode.D)) {
            if (!yon.ZitMi (Yonler.sag))
                yon = Yonler.sag;
        }

        if (Input.GetKeyDown (KeyCode.A)) {
            if (!yon.ZitMi (Yonler.sol))
                yon = Yonler.sol;
        }

        //timer değişkeni verdiğimiz hareket süresine tam bölünebildiğinde  hareketi sağlıyoruz.
        if (timer % hareketSuresi == 0) {

            /*
            Yılanın gittiği yönde yem bulunuyor mu bunun kontrolünü  yapıyoruz.
            Eğer yem dışında başka bir şeye çarparsak da yenilmiş oluyoruz. 
            yemRay yerine hareketRay ismini de kullanabilirdik.
            */
            Ray yemRay = new Ray (transform.position, yon.YonVector ());
            if (Physics.Raycast (yemRay, out RaycastHit yemHit, 1f)) {
                if (yemHit.collider.tag == "Yem") {
                    yemYedi = true;
                    //yem yerini değiştir;
                } else {
                    Sifirla ();
                }
            }

            /*
            Bu ray sayesinde zemin olarak belirlediğimiz plane dışına çıkıp çıkmadığımızı
            kontrol ediyoruz. "Hangi plane ya??" diye soruyorsanız lütfen önce README dosyasını
            okuyun.
            */
            Ray zeminRay = new Ray (transform.position, Vector3.forward);
            if (!Physics.Raycast (zeminRay)) { Sifirla (); }

            //ilk konuma varolan baş pozisyonunu kaydedip, ardından yılan başını hareket ettiriyoruz.
            konumlar[0] = transform.position;
            transform.position += yon.YonVector ();

            //her bir kuyruk parçası pozisyonlar listemizde aynı indexteki değere gidiyor.
            for (int i = 0; i < kuyruk.Count; i++) {
                kuyruk[i].transform.position = konumlar[i];
            }
            /*
            Bu kısımda yeni konumları kuyruk listemize ekleyip güncelliyoruz. 
            Bu sayede hepsi bir sonraki kuyruk parçasını takip ediyor. İlk 
            kuyruk da tabii ki başı takip ediyor.
            */
            for (int i = 1; i < konumlar.Count; i++) {
                konumlar[i] = kuyruk[i - 1].position;
            }
            /*
            Ray kontrollerinde yemYedi değeri true hale geldiyse, bu kısımda
            artık yeni kuyruk parçasını ekleyebiliriz. Bunu bütün hareketler tamamlandıktan sonra yapıyoruz ki
            kuyruk parçalarının hareketi ve takibi karışmasın.
            Kuyruğu oluşturken de en sondaki konum değerini veriyoruz. Ve sonra listelere ekliyoruz.
            */
            if (yemYedi) {
                GameObject yeniKuyruk = Instantiate (kuyrukPrefab, konumlar[konumlar.Count - 1], Quaternion.identity);
                kuyruk.Add (yeniKuyruk.transform);
                konumlar.Add (transform.position);
                yemYedi = false;
            }
        }
    }

    /*
    Oyunu ilk haline getiriyoruz.
    Burada dikkat etmemiz gereken şey Transform biçimindeki listeyi
    önce dolaşıp, içindeki elementlerin ait olduğu oyun objelerini yok etmek.
    Eğer bunu yapmazsanız, liste boşalsa bile kutucuklar varolmaya devam edecek.
     */
    void Sifirla () {
        transform.position = Vector3.zero;
        konumlar.Clear ();

        for (int i = 0; i < kuyruk.Count; i++) {
            Destroy (kuyruk[i].gameObject);
        }
        kuyruk.Clear ();
        konumlar.Add (transform.position);
    }

    /*
    Bu fonksiyonun temelde yaptığı şey yemi rastgele yeni bir konuma atamak.
    Ancak vücudun olmadığı bir noktaya atanmadığından emin olmak adına 
    kaydettiğimiz konumlarla karşılaştırıyoruz. Eğer aynı noktaya sahip bir
    konum kaydettiysek, işlemi baştan başlatıyoruz ve yeni bir 
    nokta belirliyoruz.
     */
    void YemYe (Transform yem) {
        int x;
        int y;
        bool yerlesemez;
        do {
            yerlesemez = false;
            /*
            Bu aşamada sınırları elle belirledim, ancak
            zemin olarak kullandığımız plane objesinin genişliğini 
            baz rastgele sınırlarını belirleyebilirsiniz. 
            Onun çözümünü de size bırakıyorum, pratik pratik iyidir :p
            İpucunuz Mesh.Bounds
            */
            x = Random.Range (-10, 19);
            y = Random.Range (-10, 10);
            for (int i = 0; i < konumlar.Count; i++) {
                if (konumlar[i].y == y && konumlar[i].x == x) {
                    yerlesemez = true;
                    break;
                }
            }
        } while (yerlesemez);

        yem.transform.position = new Vector3 (x, y, 0);
    }
}

//Bu static sınıf sayesinde enum objemize yeni özellikler ekleyebiliyoruz.
public static class YonlerMethod {
    /*
    Bu fonksiyon seçilen enum için bir vector değeri döndürmemizi sağlıyor.
    Önceki satırlarda transform.position için nasıl kullandığımızı görebilirsiniz.
    */
    public static Vector3 YonVector (this Yonler yon) {
        switch (yon) {
            case Yonler.yukari:
                return Vector3.up;
                break;

            case Yonler.asagi:
                return Vector3.down;
                break;

            case Yonler.sag:
                return Vector3.right;
                break;
            default:
                return Vector3.left;
                break;
        }
    }

    /*
    Yine çok işe yarar bir fonksiyon olarak, verilen yönün varolan yöne zit konumda olup olmadığını
    geri döndürüyoruz. Biraz karmaşık gelebilir. Ama enum sıralamama bakarsanız,
        0.yukari
    3.sol       2.sağ
        1.asagi
    
    şeklinde kafamızda konumlandırabiliriz. Yani tek ve çift sayılar birbirinin zıttı, bu güzel.
    Ve dikkat ederseniz, çift sayılı indexin 1 fazlası olan yön zit yönü gösteriyor.
    Ve tek sayılarda da 1 eksik değere sahip yön, zit tarafı gösteriyor. 
     */
    public static bool ZitMi (this Yonler yon, Yonler digerYon) {
        bool zit = false;
        if ((int) yon % 2 == 0) {
            if (((int) digerYon) == ((int) yon + 1)) {
                return true;
            }
        } else {
            if (((int) digerYon) == ((int) yon - 1)) {
                return true;
            }
        }
        return zit;
    }
}
//yılanın hareket edebileceği yönleri belirtmek için oluşturduğumuz enum
public enum Yonler {
    yukari,
    asagi,
    sag,
    sol
}
