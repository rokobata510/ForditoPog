Az input szöveg nevű mező egy string, ami ellenőrzésre kerül, a Szó ellenőrzés gomb megnyomására.
A tett lépések a felhasznált szabályok számaiból készült listát ír ki.
A teljes levezetés az összes megtett lépést, és az akkori állapotot írja ki egy ListBox-ba.
A szabályok ListBox-ban vannak a jelenleg tárolt szabályok, alapértelmezetten a Bin/debug/Rules.txt, ez tartalmazza a beadandó spec-ben megadott szabályokat.
példa a szintaxisra:
 - t(;(fť,4)
    - "t(" : A két olvasott karakter. 
      - "t" az állapot.
      - "(" az input string-ből olvasott karakter.
    - "fť" : A beolvasott állapotot (Ez esetben "t") erre/ezekre cseréljük.
 	- "4" : A jelenleg alkalmazni kívánt szabály száma.
    - ";" : Elválasztja a szabály feltételét, és magát a szabályt.

