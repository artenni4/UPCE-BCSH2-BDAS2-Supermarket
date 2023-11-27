CREATE OR REPLACE TRIGGER DODAVATELE_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON DODAVATELE
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DODAVATELE','INSERT' || '; dodavatel_id:' || :NEW.dodavatel_id || '; nazev:' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DODAVATELE','UPDATE' || '; dodavatel_id:' || :OLD.dodavatel_id || ' -> ' || :NEW.dodavatel_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DODAVATELE','DELETE' || '; dodavatel_id:' || :OLD.dodavatel_id || '; nazev:' || :OLD.nazev,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER DRUHY_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON DRUHY_ZBOZI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DRUHY_ZBOZI','INSERT' || '; druh_zbozi_id:' || :NEW.druh_zbozi_id || '; nazev:' || :NEW.nazev || '; popis:' || :NEW.popis,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DRUHY_ZBOZI','UPDATE' || '; druh_zbozi_id:' || :OLD.druh_zbozi_id || ' -> ' || :NEW.druh_zbozi_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev || '; popis:' || :OLD.popis || ' -> ' || :NEW.popis,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DRUHY_ZBOZI','DELETE' || '; druh_zbozi_id:' || :OLD.druh_zbozi_id || '; nazev:' || :OLD.nazev || '; popis:' || :OLD.popis,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER MERNE_JEDNOTKY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON MERNE_JEDNOTKY
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MERNE_JEDNOTKY','INSERT' || '; merna_jednotka_id:' || :NEW.merna_jednotka_id || '; nazev:' || :NEW.nazev || '; zkratka:' || :NEW.zkratka,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MERNE_JEDNOTKY','UPDATE' || '; merna_jednotka_id:' || :OLD.merna_jednotka_id || ' -> ' || :NEW.merna_jednotka_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev || '; zkratka:' || :OLD.zkratka || ' -> ' || :NEW.zkratka,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MERNE_JEDNOTKY','DELETE' || '; merna_jednotka_id:' || :OLD.merna_jednotka_id || '; nazev:' || :OLD.nazev || '; zkratka:' || :OLD.zkratka,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER MISTA_ULOZENI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON MISTA_ULOZENI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MISTA_ULOZENI','INSERT' || '; misto_ulozeni_id:' || :NEW.misto_ulozeni_id || '; kod:' || :NEW.kod || '; poloha:' || :NEW.poloha || '; supermarket_id:' || :NEW.supermarket_id || '; misto_ulozeni_typ:' || :NEW.misto_ulozeni_typ,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MISTA_ULOZENI','UPDATE' || '; misto_ulozeni_id:' || :OLD.misto_ulozeni_id || ' -> ' || :NEW.misto_ulozeni_id || '; kod:' || :OLD.kod || ' -> ' || :NEW.kod || '; poloha:' || :OLD.poloha || ' -> ' || :NEW.poloha || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; misto_ulozeni_typ:' || :OLD.misto_ulozeni_typ || ' -> ' || :NEW.misto_ulozeni_typ,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MISTA_ULOZENI','DELETE' || '; misto_ulozeni_id:' || :OLD.misto_ulozeni_id || '; kod:' || :OLD.kod || '; poloha:' || :OLD.poloha || '; supermarket_id:' || :OLD.supermarket_id || '; misto_ulozeni_typ:' || :OLD.misto_ulozeni_typ,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PLATBA_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PLATBA
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PLATBA','INSERT' || '; castka:' || :NEW.castka || '; prodej_id:' || :NEW.prodej_id || '; typ_placeni_id:' || :NEW.typ_placeni_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PLATBA','UPDATE' || '; castka:' || :OLD.castka || ' -> ' || :NEW.castka || '; prodej_id:' || :OLD.prodej_id || ' -> ' || :NEW.prodej_id || '; typ_placeni_id:' || :OLD.typ_placeni_id || ' -> ' || :NEW.typ_placeni_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PLATBA','DELETE' || '; castka:' || :OLD.castka || '; prodej_id:' || :OLD.prodej_id || '; typ_placeni_id:' || :OLD.typ_placeni_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER POKLADNY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON POKLADNY
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('POKLADNY','INSERT' || '; pokladna_id:' || :NEW.pokladna_id || '; supermarket_id:' || :NEW.supermarket_id || '; nazev:' || :NEW.nazev || '; kod:' || :NEW.kod || '; poznamky:' || :NEW.poznamky,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('POKLADNY','UPDATE' || '; pokladna_id:' || :OLD.pokladna_id || ' -> ' || :NEW.pokladna_id || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev || '; kod:' || :OLD.kod || ' -> ' || :NEW.kod || '; poznamky:' || :OLD.poznamky || ' -> ' || :NEW.poznamky,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('POKLADNY','DELETE' || '; pokladna_id:' || :OLD.pokladna_id || '; supermarket_id:' || :OLD.supermarket_id || '; nazev:' || :OLD.nazev || '; kod:' || :OLD.kod || '; poznamky:' || :OLD.poznamky,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PRODANE_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PRODANE_ZBOZI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODANE_ZBOZI','INSERT' || '; prodej_id:' || :NEW.prodej_id || '; celkova_cena:' || :NEW.celkova_cena || '; kusy:' || :NEW.kusy || '; supermarket_id:' || :NEW.supermarket_id || '; zbozi_id:' || :NEW.zbozi_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODANE_ZBOZI','UPDATE' || '; prodej_id:' || :OLD.prodej_id || ' -> ' || :NEW.prodej_id || '; celkova_cena:' || :OLD.celkova_cena || ' -> ' || :NEW.celkova_cena || '; kusy:' || :OLD.kusy || ' -> ' || :NEW.kusy || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; zbozi_id:' || :OLD.zbozi_id || ' -> ' || :NEW.zbozi_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODANE_ZBOZI','DELETE' || '; prodej_id:' || :OLD.prodej_id || '; celkova_cena:' || :OLD.celkova_cena || '; kusy:' || :OLD.kusy || '; supermarket_id:' || :OLD.supermarket_id || '; zbozi_id:' || :OLD.zbozi_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PRODAVANE_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PRODAVANE_ZBOZI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODAVANE_ZBOZI','INSERT' || '; zbozi_id:' || :NEW.zbozi_id || '; supermarket_id:' || :NEW.supermarket_id || '; aktivni:' || :NEW.aktivni,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODAVANE_ZBOZI','UPDATE' || '; zbozi_id:' || :OLD.zbozi_id || ' -> ' || :NEW.zbozi_id || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; aktivni:' || :OLD.aktivni || ' -> ' || :NEW.aktivni,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODAVANE_ZBOZI','DELETE' || '; zbozi_id:' || :OLD.zbozi_id || '; supermarket_id:' || :OLD.supermarket_id || '; aktivni:' || :OLD.aktivni,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PRODEJE_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PRODEJE
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODEJE','INSERT' || '; prodej_id:' || :NEW.prodej_id || '; datum:' || :NEW.datum || '; pokladna_id:' || :NEW.pokladna_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODEJE','UPDATE' || '; prodej_id:' || :OLD.prodej_id || ' -> ' || :NEW.prodej_id || '; datum:' || :OLD.datum || ' -> ' || :NEW.datum || '; pokladna_id:' || :OLD.pokladna_id || ' -> ' || :NEW.pokladna_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODEJE','DELETE' || '; prodej_id:' || :OLD.prodej_id || '; datum:' || :OLD.datum || '; pokladna_id:' || :OLD.pokladna_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER REGIONY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON REGIONY
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('REGIONY','INSERT' || '; region_id:' || :NEW.region_id || '; nazev:' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('REGIONY','UPDATE' || '; region_id:' || :OLD.region_id || ' -> ' || :NEW.region_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('REGIONY','DELETE' || '; region_id:' || :OLD.region_id || '; nazev:' || :OLD.nazev,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ROLE_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ROLE
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE','INSERT' || '; role_id:' || :NEW.role_id || '; nazev:' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE','UPDATE' || '; role_id:' || :OLD.role_id || ' -> ' || :NEW.role_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE','DELETE' || '; role_id:' || :OLD.role_id || '; nazev:' || :OLD.nazev,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ROLE_ZAMESTNANCU_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ROLE_ZAMESTNANCU
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE_ZAMESTNANCU','INSERT' || '; role_id:' || :NEW.role_id || '; zamestnanec_id:' || :NEW.zamestnanec_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE_ZAMESTNANCU','UPDATE' || '; role_id:' || :OLD.role_id || ' -> ' || :NEW.role_id || '; zamestnanec_id:' || :OLD.zamestnanec_id || ' -> ' || :NEW.zamestnanec_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE_ZAMESTNANCU','DELETE' || '; role_id:' || :OLD.role_id || '; zamestnanec_id:' || :OLD.zamestnanec_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER SOUBORY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON SOUBORY
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SOUBORY','INSERT' || '; soubor_id:' || :NEW.soubor_id || '; nazev_souboru:' || :NEW.nazev_souboru || '; pripona:' || :NEW.pripona || '; datum_nahrani:' || :NEW.datum_nahrani || '; supermarket_id:' || :NEW.supermarket_id || '; zamestnanec_id:' || :NEW.zamestnanec_id || '; datum_modifikace:' || :NEW.datum_modifikace,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SOUBORY','UPDATE' || '; soubor_id:' || :OLD.soubor_id || ' -> ' || :NEW.soubor_id || '; nazev_souboru:' || :OLD.nazev_souboru || ' -> ' || :NEW.nazev_souboru || '; pripona:' || :OLD.pripona || ' -> ' || :NEW.pripona || '; datum_nahrani:' || :OLD.datum_nahrani || ' -> ' || :NEW.datum_nahrani || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; zamestnanec_id:' || :OLD.zamestnanec_id || ' -> ' || :NEW.zamestnanec_id || '; datum_modifikace:' || :OLD.datum_modifikace || ' -> ' || :NEW.datum_modifikace,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SOUBORY','DELETE' || '; soubor_id:' || :OLD.soubor_id || '; nazev_souboru:' || :OLD.nazev_souboru || '; pripona:' || :OLD.pripona || '; datum_nahrani:' || :OLD.datum_nahrani || '; supermarket_id:' || :OLD.supermarket_id || '; zamestnanec_id:' || :OLD.zamestnanec_id || '; datum_modifikace:' || :OLD.datum_modifikace,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER SUPERMARKETY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON SUPERMARKETY
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SUPERMARKETY','INSERT' || '; supermarket_id:' || :NEW.supermarket_id || '; adresa:' || :NEW.adresa || '; region_id:' || :NEW.region_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SUPERMARKETY','UPDATE' || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; adresa:' || :OLD.adresa || ' -> ' || :NEW.adresa || '; region_id:' || :OLD.region_id || ' -> ' || :NEW.region_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SUPERMARKETY','DELETE' || '; supermarket_id:' || :OLD.supermarket_id || '; adresa:' || :OLD.adresa || '; region_id:' || :OLD.region_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER TYPY_PLACENI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON TYPY_PLACENI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('TYPY_PLACENI','INSERT' || '; typ_placeni_id:' || :NEW.typ_placeni_id || '; nazev:' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('TYPY_PLACENI','UPDATE' || '; typ_placeni_id:' || :OLD.typ_placeni_id || ' -> ' || :NEW.typ_placeni_id || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('TYPY_PLACENI','DELETE' || '; typ_placeni_id:' || :OLD.typ_placeni_id || '; nazev:' || :OLD.nazev,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ULOZENI_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ULOZENI_ZBOZI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ULOZENI_ZBOZI','INSERT' || '; kusy:' || :NEW.kusy || '; misto_ulozeni_id:' || :NEW.misto_ulozeni_id || '; supermarket_id:' || :NEW.supermarket_id || '; zbozi_id:' || :NEW.zbozi_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ULOZENI_ZBOZI','UPDATE' || '; kusy:' || :OLD.kusy || ' -> ' || :NEW.kusy || '; misto_ulozeni_id:' || :OLD.misto_ulozeni_id || ' -> ' || :NEW.misto_ulozeni_id || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; zbozi_id:' || :OLD.zbozi_id || ' -> ' || :NEW.zbozi_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ULOZENI_ZBOZI','DELETE' || '; kusy:' || :OLD.kusy || '; misto_ulozeni_id:' || :OLD.misto_ulozeni_id || '; supermarket_id:' || :OLD.supermarket_id || '; zbozi_id:' || :OLD.zbozi_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ZAMESTNANCI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ZAMESTNANCI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZAMESTNANCI','INSERT' || '; zamestnanec_id:' || :NEW.zamestnanec_id || '; login:' || :NEW.login || '; jmeno:' || :NEW.jmeno || '; prijmeni:' || :NEW.prijmeni || '; datum_nastupu:' || :NEW.datum_nastupu || '; supermarket_id:' || :NEW.supermarket_id || '; manazer_id:' || :NEW.manazer_id || '; rodne_cislo:' || :NEW.rodne_cislo,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZAMESTNANCI','UPDATE' || '; zamestnanec_id:' || :OLD.zamestnanec_id || ' -> ' || :NEW.zamestnanec_id || '; login:' || :OLD.login || ' -> ' || :NEW.login || '; jmeno:' || :OLD.jmeno || ' -> ' || :NEW.jmeno || '; prijmeni:' || :OLD.prijmeni || ' -> ' || :NEW.prijmeni || '; datum_nastupu:' || :OLD.datum_nastupu || ' -> ' || :NEW.datum_nastupu || '; supermarket_id:' || :OLD.supermarket_id || ' -> ' || :NEW.supermarket_id || '; manazer_id:' || :OLD.manazer_id || ' -> ' || :NEW.manazer_id || '; rodne_cislo:' || :OLD.rodne_cislo || ' -> ' || :NEW.rodne_cislo,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZAMESTNANCI','DELETE' || '; zamestnanec_id:' || :OLD.zamestnanec_id || '; login:' || :OLD.login || '; jmeno:' || :OLD.jmeno || '; prijmeni:' || :OLD.prijmeni || '; datum_nastupu:' || :OLD.datum_nastupu || '; supermarket_id:' || :OLD.supermarket_id || '; manazer_id:' || :OLD.manazer_id || '; rodne_cislo:' || :OLD.rodne_cislo,SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ZBOZI
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZBOZI','INSERT' || '; zbozi_id:' || :NEW.zbozi_id || '; druh_zbozi_id:' || :NEW.druh_zbozi_id || '; merna_jednotka_id:' || :NEW.merna_jednotka_id || '; navahu:' || :NEW.navahu || '; nazev:' || :NEW.nazev || '; cena:' || :NEW.cena || '; carovykod:' || :NEW.carovykod || '; popis:' || :NEW.popis || '; dodavatel_id:' || :NEW.dodavatel_id,SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZBOZI','UPDATE' || '; zbozi_id:' || :OLD.zbozi_id || ' -> ' || :NEW.zbozi_id || '; druh_zbozi_id:' || :OLD.druh_zbozi_id || ' -> ' || :NEW.druh_zbozi_id || '; merna_jednotka_id:' || :OLD.merna_jednotka_id || ' -> ' || :NEW.merna_jednotka_id || '; navahu:' || :OLD.navahu || ' -> ' || :NEW.navahu || '; nazev:' || :OLD.nazev || ' -> ' || :NEW.nazev || '; cena:' || :OLD.cena || ' -> ' || :NEW.cena || '; carovykod:' || :OLD.carovykod || ' -> ' || :NEW.carovykod || '; popis:' || :OLD.popis || ' -> ' || :NEW.popis || '; dodavatel_id:' || :OLD.dodavatel_id || ' -> ' || :NEW.dodavatel_id,SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZBOZI','DELETE' || '; zbozi_id:' || :OLD.zbozi_id || '; druh_zbozi_id:' || :OLD.druh_zbozi_id || '; merna_jednotka_id:' || :OLD.merna_jednotka_id || '; navahu:' || :OLD.navahu || '; nazev:' || :OLD.nazev || '; cena:' || :OLD.cena || '; carovykod:' || :OLD.carovykod || '; popis:' || :OLD.popis || '; dodavatel_id:' || :OLD.dodavatel_id,SYSTIMESTAMP,USER);
    END IF;
END;
/
