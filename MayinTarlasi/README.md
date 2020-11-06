# Pong
Bu kodlar Karadot isimli Youtube kanalımda oluşturduğum Unity Kodlama Yarışı serisinin Pong isimli videosunda oluşturduğum kodlardır.  
İsterseniz videoyu (https://youtu.be/YjShi6nU4yQ) izleyerek takip edebilir, isterseniz aşağıdaki yönergeyi takip ederek direkt olarak kodları kullanabilirsiniz. Videonun dışında ufak düzenlemeler yaptım ve yorum satırlarını ekledim. Kodları inceleyip yorum satırlarıyla iyice anlamaya özen göstermenizi öneririm. Ayrıca geliştirilebilir yönleri sizler için egzersiz olması amacıyla yorum satırlarında belirttim.

## Nasıl kullanılır ?
1. 3 adet sprite oluşturun ve texture dosyalarını atayın.   
   * Eğer kullanacağınız bir sprite dosyanız yoksa proje panelinde boş bir alana sağ tıklayıp Create>Sprites altından istediğiniz bir hazır görseli oluşturun.
2. "Top", "Sağ Kol", "Sol Kol" şeklinde isimlendirin. Kol objelerinin scale y değerini istediğiniz büyüklüğe göre değiştirin.
3. KontrolEdilebilirHareketli isimli bileşenleri kol objelerine atayın. Yon ekseni değerini istediğiniz değer ile değiştirmeyi unutmayın. Sağ için "Mouse Y" sol için de "Vertical" değerlerini verebilirsiniz.
4. SabitHareketli bileşenini top objenize ekleyin.
5. 2 adet UI Text oluşturun. "SolSkor" ve "SağSkor" olarak adlandırın. Ardından solSkor için sol üste dayalı ve metin de sola dayalı şekilde ayarlayın. Sağ skor için de tam tersi şekilde sağa üste anchor halde ve sağa dayalı metin şeklinde ayarlama yapın.
6. "Main Camera" objenize ya da farklı isimliyse camera objenize PongKontrol isimli bileşeni ekleyin. 
7.  PongKontrol bileşeni içerisinde belirtilen alanlara gerekli objeleri sürükleyin. İsimlendirmelerin açık olduğunu düşünüyorum, hangisi nereye gelecek anlayacaksınız.
8.  "Play" tuşuna basın ve oynamaya başlayın.
9.  Sevmediğiniz şeyler mi var ? İstediğiniz geliştirmeyi yapabilirsiniz. Daha önce de belirttiğim gibi bazı geliştirilmeye açık şeylere dair açıklamalarda bilgilendirmeler yaptım. Şayet varolan kodlarla ilgili bir sorun yaşıyorsanız da ulaşabilirsiniz.
