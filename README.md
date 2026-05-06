Jednoduchá multiplayer textová hra v C# (TCP client-server), kde hráči prozkoumávají svět, obchodují a plní questy.

Funkce
Svět
Více místností (klub, park, trh, ulice)

Pohyb mezi lokacemi (jdi <místo>)

Popis místnosti (prozkoumat)

Komunikace
rekni <text> – zpráva hráčům ve stejné místnosti

krik <text> – zpráva všem hráčům

notifikace o připojení a odpojení hráče

Inventář
inventar – zobrazí inventář

vezmi <item> – sebere item

zahod <item> – odstraní item

Obchodování
kup <item> – nákup od dealera

prodej <item> – prodej zákazníkovi

ceny a nabídka jsou načítány z trades.json

NPC
mluv <npc> – interakce s NPC

NPC mají vlastní dialogy

NPC se pohybují mezi místnostmi

Questy
jednoduchý quest systém

např. doručení balíčku do určité lokace

odměny ve formě peněz nebo itemů

Struktura projektu
MVP/
├── Server/
│   ├── GameManager.cs
│   ├── CommandHandler.cs
│   ├── GameServer.cs
│   ├── ClientHandler.cs
│   ├── WorldLoader.cs
│   └── data/
│       ├── rooms.json
│       ├── items.json
│       ├── npcs.json
│       ├── trades.json
│       ├── quests.json
│       └── dialogues.json




Spuštění
Server
dotnet run
Server běží na adrese:

localhost:5001
Client
Spusť ve druhém terminálu:

dotnet run
Konfigurace
UTF-8 (diakritika)
Na začátek programu:



Příklad použití
Zadej jméno:
Patrik

Vítej Patrik

> prozkoumat
== Noční klub ==
Hlasitá hudba, lidé tančí
NPC: dealer

> mluv dealer
Prodávám: prasek (100 Kč)

> kup prasek
Koupil jsi prasek za 100 Kč

> jdi trh

> prodej prasek
Prodal jsi prasek za 100 Kč


více NPC a lokací

Autor
Patrik Papuča a Jonáš Raichl
