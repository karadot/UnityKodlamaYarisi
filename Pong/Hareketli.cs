using UnityEngine;

/*
Üreteceğimiz hareketli nesneler için yarattığımız 
abstract(temel) sınıf. Burada olan bütün yapılar
bunu miras alan sınıflarda da olacaktır.
*/
public abstract class Hareketli : MonoBehaviour {
    float _alt, _ust, _sag, _sol;

    Vector2 yon;

    public float Alt { get => _alt; set => _alt = value; }
    public float Ust { get => _ust; set => _ust = value; }
    public float Sag { get => _sag; set => _sag = value; }
    public float Sol { get => _sol; set => _sol = value; }
    public Vector2 Yon { get => yon; set => yon = value; }

    public abstract void HareketEt ();
    public abstract void Sinirlar (Vector2 solAlt, Vector2 sagUst);
    public abstract void SinirlariKontrolEt (ref Vector3 hedefKonum);
}
