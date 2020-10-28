using UnityEngine;
using UnityEngine.UI;

public class MayinKutulari : MonoBehaviour {

    /*
    Her bir kutunun satır ve sütun değerlerini bu değişkende tutarak,
    grid üzerindeki yer bilgisini de saklamış oluyoruz.
    */
    public int indexX, indexY;

    //Mayın olup olmama durumunu tuttuğumuz değişken
    public bool MayinMi = false;

    Button button;
    Text text;

    /*
    Tıkladıktan sonra buton etkileşimini kapatacağımız için, ayrı bir değişken ataması ile uğraşmak yerine
    bu şekilde butonun etkileşim değerini kontrol ediyorum.
    Eğer etkileşilebilir halde iste tıklanabilir haldedir.
     */
    public bool Tiklanabilir {
        get {
            return button.interactable;
        }
    }

    //Gerekli bileşenlere erişip text bileşenini boş hale getiriyorum.
    void Start () {
        button = GetComponent<Button> ();
        text = GetComponentInChildren<Text> ();
        text.text = "";
    }
    //Kutunun tıklanabilrliğini kapatıp MayinKontrol üzerinden gelen komşu sayısını text değişkenine atıyoruz.
    public void KutuyuGoster (string yazi) {
        button.interactable = false;
        text.text = yazi;
    }
    //Tekrar tıklanabilir hale getirip text içeriğini boş hale getiriyoruz.
    public void KutuyuGizle () {
        button.interactable = true;
        text.text = "";
    }
}
