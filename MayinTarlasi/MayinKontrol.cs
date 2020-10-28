using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MayinKontrol : MonoBehaviour {

    /*
    Satır ve sütunları oluşturmak için kullanacağımız prefablar.
    Ayrıntılı bilgi için README dosyasını okuyun.
    */
    [SerializeField]
    GameObject sira, kutu;
    //Oluşturacağımız objelerin ebeveyni olacak obje
    [SerializeField]
    Transform oyunPanel;

    /*
    Oluşturulacak grid yapısı kaça kaç olacak bunu belirlediğimiz değişken.
    Şu haliyle 5x5 bir grid oluşturacaktır.
    */
    [SerializeField]
    int boyut = 5;

    /*
    Yarattığımız kutuları liste olarak tuttuğumuz değişken. 
    2 boyutlu oluşturmamızın sebebi yatay ve dikey kontrolleri kolaylaştırmak
    */
    MayinKutulari[, ] kutuListe;

    /*
    Oyunun yanma durumunu belirlediğimiz sistem.
    Kazanma durumunu, yani hiçbir mayına tıklamadan
    kalan kutuları açma durumunu size bırakıyorum. Ufak bir 
    ipucu olarak, mayın olmayan kutular açılmış mı bunu kontrol edebilirsiniz.
    Örneğin, başta bu kutu indexlerini bir listede tutup, sonrasında açılan kutunun 
    index değerlerini vererek bu listeyi boşaltabilir ve listedeki eleman sayısının
    0'a gelip gelmediğini kontrol edebilirsiniz.
    */
    bool OyunBitti = false;

    void Start () {
        /*
        Kutu listemizi tutacağımız değişkenin tanımlamasını yapıyoruz. Burada dizi
        kullandığımız için, boyutları baştan vermemiz gerekiyor.
        */
        kutuListe = new MayinKutulari[boyut, boyut];

        //Öncelikle sira objesini oluşturuyoruz.
        for (int i = 0; i < boyut; i++) {
            GameObject yeniSira = Instantiate (sira, oyunPanel);
            /*
            Ardından herbiri sıra içinde bulunan kutucukları oluşturup
            az önce oluşturduğumuz sıranın altobjesi haline getiriyoruz.
            */
            for (int j = 0; j < boyut; j++) {
                GameObject yeniKutu = Instantiate (kutu, yeniSira.transform);

                MayinKutulari kutuCom = yeniKutu.GetComponent<MayinKutulari> ();
                kutuCom.indexX = i;
                kutuCom.indexY = j;
                //yarattığımız kutuyu listeye kaydediyoruz.
                kutuListe[i, j] = kutuCom;
                /*
                kutunun mayın olma durumunu rastgele olarak belirliyoruz.
                Burada size ufak bir pratik olması amacıyla da örnek düşündüm.
                Mayın sayısını bu şekilde belirlemek yerine, sabit bir sayıda olmasını nasıl sağlarsınız 
                bunu bir düşünün. Şu anki haliyle düşük de olsa kutuların hepsi mayın olabilir, ya da yarısı.
                Bir sayı belirleyip bu sayı adedinde mayın yerleştirmeyi deneyin. Ama rastgeleliği de
                geri plana atmayın.
                */
                kutuCom.MayinMi = Random.Range (0f, 1f) >.75f;

                /*
                Son olarak yarattığımı kutu, aslında button özelliği içeren objenin
                tıklanma olayına fonksiyonumuzu ekliyoruz. 
                */
                yeniKutu.GetComponent<Button> ().onClick.AddListener (() => {
                    kutuyuAc (kutuCom);
                });
            }
        }
    }

    void Update () {
        //Oyun bittiyse objelerin değerlerini sıfırlayıp mayinlari tekrar rastgele oluşturuyoruz
        if (OyunBitti && Input.GetKeyUp (KeyCode.Space)) {
            KutulariYenidenOlustur ();
        }
    }

    /*
    Burada temel olarak yaptığımız şey, kutunun mayin olma durumunu kontrol etmek, öyleyse bütün kutuları göstermek.
    Eğer mayın değilse de, komşularına bakıp etrafında kaç mayin var bunu kontrol ediyoruz.
    Hiç mayın yoksa o kutuları da açıyoruz. Komşuda mayına denk gelene kadar bu döngü devam ediyor.
     */
    void kutuyuAc (MayinKutulari kutu) {
        if (kutu.MayinMi) {
            OyunBitti = true;
            ButunKutulariAc ();
        } else if (kutu.Tiklanabilir) {
            int komsuMayin = KomsuMayinBul (kutu.indexX, kutu.indexY);
            kutu.KutuyuGoster (komsuMayin.ToString ());
            if (komsuMayin == 0) {
                KomsulariAc (kutu.indexX, kutu.indexY);
            }
        }
    }

    //Kutuları tekrar tıklanabilir hale getirip, mayın olma ve olmama durumlarını baştan belirliyoruz.
    void KutulariYenidenOlustur () {
        for (int i = 0; i < boyut; i++) {
            for (int j = 0; j < boyut; j++) {
                kutuListe[i, j].KutuyuGizle ();
                kutuListe[i, j].MayinMi = Random.Range (0f, 1f) >.75f;
            }
        }
    }

    /*
    Burada yaptığımız da aslında gayet basit bir içiçe döngü. Her bir satır ve sütunu
    kontrol ediyoruz. Mayın değilse komşularına bakıp mayın sayısını yazdırıyoruz.
     */
    void ButunKutulariAc () {
        for (int i = 0; i < boyut; i++) {

            for (int j = 0; j < boyut; j++) {
                if (kutuListe[i, j].MayinMi)
                    kutuListe[i, j].KutuyuGoster ("X");
                else {
                    int sayi = KomsuMayinBul (i, j);
                    kutuListe[i, j].KutuyuGoster (sayi.ToString ());
                }
            }
        }
    }

    /*
    Verdiğimiz x ve y konumundaki komşuları sırasıyla dolanıp açıyoruz.
     */
    void KomsulariAc (int x, int y) {
        List<MayinKutulari> komsular = Komsular (x, y);

        foreach (MayinKutulari item in komsular) {
            if (item.Tiklanabilir)
                kutuyuAc (item);
        }
    }

    /*
    Bu içiçe döngüde ise yaptığımız birer birim mesafedeki komşuları kontrol etmek.
    Örneğin 3.satır 4. sütundaki kutunun sol üstündeki kutuyu bulmak için
    2.satır ve 3.sütuna bakmamız gerekiyor. Yani (3-1) ve (4-3) indexler.
    Sağ alttaki satır ise 4.satır 5.sütun olacak. yani (3+1) ve (4+1). birer adım öncesi ve sonrası kısacası.
    Umarım for döngüsünde bu mantığı nasıl kullandığımı anlamışsınızdır.
     */
    List<MayinKutulari> Komsular (int x, int y) {
        List<MayinKutulari> komsular = new List<MayinKutulari> ();
        for (int i = x - 1; i <= x + 1; i++) {
            /*
            bizim satır sütun sayımız index olarak minimum 0 ve maksimim boyut-1 değerini alabilir.
            Eğer bunların dışına çıkan değerlere erişmeye çalışırsak hata alırız. Bu durumlarda
            continue diyip o satır sütünu atlıyoruz.
            */
            if (i < 0 || i >= boyut)
                continue;
            for (int j = y - 1; j <= y + 1; j++) {
                if (j < 0 || j >= boyut)
                    continue;
                //Eğer tıklanabilir bir komşu seçildi ise, bunu listeye ekliyoruz.
                if (kutuListe[i, j].Tiklanabilir)
                    komsular.Add (kutuListe[i, j]);
            }
        }
        return komsular;
    }

    //Komsular fonksiyonuna benzer şekilde çalışıyor. Ancak mayın olan bir komşu varsa değişkenimiz arttırıyoruz ve mayın sayısını elde ediyoruz böylece.
    int KomsuMayinBul (int x, int y) {
        int sayi = 0;
        for (int i = x - 1; i <= x + 1; i++) {
            if (i < 0 || i >= boyut)
                continue;
            for (int j = y - 1; j <= y + 1; j++) {
                if (j < 0 || j >= boyut)
                    continue;
                if (kutuListe[i, j].MayinMi)
                    sayi++;
            }
        }
        return sayi;
    }
}
