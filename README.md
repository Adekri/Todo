# Todo

Aplikace TODO list spravuje seznam úkolů skládající se z:
- Hodnoty Id, podle které například vyhledává v databázi úkol, který má být smazán.
- Data vytvoření, které se nastavuje automaticky.
- Data dokončení, které nastavuje uživatel při tvorbě nového úkolu.
- Stavu, tedy hodnoty Todo/Done, která se u nového úkolu nastaví na Todo, ale uživatel ji může měnit.
- Samotného obsahu úkolu, které je ve formě textu a zadává ho opět uživatel při vytvoření úkolu.

Aplikace umožňuje:
- vytvářet nové úkoly,
- mazat úkoly,
- měnit stav úkolu Todo/Done výběrem možnosti po stisknutí pravého tlačítka na úkol,
- zobrazit pouze dokončené nebo pouze nedokončené úkoly,
- smazat libovolný úkol dvojitým poklepáním,
- smazat všechny dokončené úkoly,
- zobrazit text úkolu (například pokud je příliš dlouhý) v novém okně výběrem možnosti po stisknutí pravého tlačítka.

Aplikace obsahuje vlastní databázi SQLite, kterou obsluuhjí metody Database.cs a DataAccess.cs.
