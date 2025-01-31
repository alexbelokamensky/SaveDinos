[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/anScgbOi)
# Sarcina2

### Dezvoltarea unui joc de tip „Catch the Falling Objects” în WPF
#### Obiectiv:
Creați un joc simplu în WPF în care utilizatorul trebuie să prindă obiectele care cad de sus în jos pe ecran. Jocul va folosi animații și controale grafice pentru a oferi o experiență interactivă.

#### Cerințe detaliate:

##### 1. Interfață utilizator:
  - Utilizați un Canvas pentru a afișa elementele grafice.
  - Adăugați un TextBlock sau Label pentru a afișa scorul curent al utilizatorului.
  - Un Button pentru a începe sau reseta jocul.
##### 2. Funcționalitate:
  - Obiectele trebuie să cadă aleatoriu de sus în jos pe ecran, iar utilizatorul trebuie să le prindă mutând un element (de exemplu, o bară sau un coș) în stânga și dreapta folosind tastele săgeți de pe tastatură.
  - Dacă un obiect este prins, utilizatorul primește puncte, iar dacă obiectul ajunge jos fără a fi prins, utilizatorul pierde puncte.
  - Jocul trebuie să se termine după un număr prestabilit de încercări (de exemplu, 5 obiecte ratate).
##### 3. Animații:
  - Folosiți Storyboard pentru a crea animații care fac ca obiectele să cadă pe ecran.
  - Controlați viteza căderii în funcție de progresul jocului (creșteți dificultatea pe măsură ce utilizatorul acumulează puncte).
##### 4. Scor și feedback:
  - Implementați un sistem de scor care afișează punctajul curent al jucătorului în timp real.
  - Oferiți feedback vizual atunci când un obiect este prins sau ratat (de exemplu, schimbarea culorii pentru obiectele ratate).
##### 5. Persistența scorurilor:
  - Salvați cel mai bun scor obținut de utilizator într-un fișier JSON sau XML și afișați-l la începutul jocului.
##### 6. Design și stilizare:
  - Stilizați elementele jocului folosind Template-uri și Resurse WPF pentru a crea o interfață atractivă și consecventă.
  - Utilizați culori, animații și efecte vizuale pentru a îmbunătăți experiența jucătorului.
##### 7. Prezentare:
  - Studenții trebuie să prezinte jocul final, explicând funcționalitățile, structura codului și modul în care au implementat logica jocului și animațiile.
