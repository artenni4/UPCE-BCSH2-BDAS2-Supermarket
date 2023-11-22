CREATE TABLE dodavatele (
    dodavatel_id NUMBER(3) NOT NULL,
    nazev        NVARCHAR2(50) NOT NULL
);

ALTER TABLE dodavatele ADD CONSTRAINT dodavatele_pk PRIMARY KEY ( dodavatel_id );

CREATE TABLE druhy_zbozi (
    druh_zbozi_id NUMBER(4) NOT NULL,
    nazev         NVARCHAR2(30) NOT NULL,
    popis         NVARCHAR2(100)
);

ALTER TABLE druhy_zbozi ADD CONSTRAINT druhy_zbozi_pk PRIMARY KEY ( druh_zbozi_id );

CREATE TABLE logy (
    tabulka  VARCHAR2(32) NOT NULL,
    operace  VARCHAR2(20) NOT NULL,
    cas      TIMESTAMP WITH LOCAL TIME ZONE NOT NULL,
    uzivatel VARCHAR2(30) NOT NULL
);

CREATE TABLE merne_jednotky (
    merna_jednotka_id NUMBER(3) NOT NULL,
    nazev             NVARCHAR2(20) NOT NULL,
    zkratka           NVARCHAR2(10) NOT NULL
);

ALTER TABLE merne_jednotky ADD CONSTRAINT merne_jednotky_pk PRIMARY KEY ( merna_jednotka_id );

CREATE TABLE mista_ulozeni (
    misto_ulozeni_id  NUMBER(6) NOT NULL,
    kod               VARCHAR2(10) NOT NULL,
    poloha            NVARCHAR2(100),
    supermarket_id    NUMBER(5) NOT NULL,
    misto_ulozeni_typ VARCHAR2(5) NOT NULL
);

ALTER TABLE mista_ulozeni
    ADD CONSTRAINT ch_inh_mista_ulozeni CHECK ( misto_ulozeni_typ IN ( 'JINE', 'PULT', 'SKLAD' ) );

ALTER TABLE mista_ulozeni ADD CONSTRAINT mista_ulozeni_pk PRIMARY KEY ( misto_ulozeni_id );

ALTER TABLE mista_ulozeni ADD CONSTRAINT mst_ulzn_kod_spmkt_id_un UNIQUE ( kod,
                                                                           supermarket_id );

CREATE TABLE platba (
    castka         NUMBER(9, 2) NOT NULL,
    prodej_id      NUMBER(10) NOT NULL,
    typ_placeni_id NUMBER(3) NOT NULL
);

ALTER TABLE platba ADD CONSTRAINT platba_pk PRIMARY KEY ( prodej_id,
                                                          typ_placeni_id );

CREATE TABLE pokladny (
    pokladna_id    NUMBER(6) NOT NULL,
    supermarket_id NUMBER(5) NOT NULL,
    nazev          NVARCHAR2(30) NOT NULL,
    kod            VARCHAR2(10) NOT NULL,
    poznamky       NVARCHAR2(100)
);

ALTER TABLE pokladny ADD CONSTRAINT pokladny_pk PRIMARY KEY ( pokladna_id );

ALTER TABLE pokladny ADD CONSTRAINT pokladny_kod_supermarket_id_un UNIQUE ( kod,
                                                                            supermarket_id );

CREATE TABLE prodane_zbozi (
    prodej_id      NUMBER(10) NOT NULL,
    celkova_cena   NUMBER(8, 2) NOT NULL,
    kusy           NUMBER(6, 3) NOT NULL,
    supermarket_id NUMBER(5) NOT NULL,
    zbozi_id       NUMBER(9) NOT NULL
);

ALTER TABLE prodane_zbozi
    ADD CONSTRAINT prodane_zbozi_pk PRIMARY KEY ( supermarket_id,
                                                  zbozi_id,
                                                  prodej_id );

CREATE TABLE prodavane_zbozi (
    zbozi_id       NUMBER(9) NOT NULL,
    supermarket_id NUMBER(5) NOT NULL,
    aktivni        NUMBER(1) DEFAULT 0 NOT NULL
);

ALTER TABLE prodavane_zbozi
    ADD CHECK ( aktivni IN ( 0, 1 ) );

ALTER TABLE prodavane_zbozi ADD CONSTRAINT prodavane_zbozi_pk PRIMARY KEY ( supermarket_id,
                                                                            zbozi_id );

CREATE TABLE prodeje (
    prodej_id   NUMBER(10) NOT NULL,
    datum       TIMESTAMP WITH TIME ZONE NOT NULL,
    pokladna_id NUMBER(6) NOT NULL
);

ALTER TABLE prodeje ADD CONSTRAINT prodeje_pk PRIMARY KEY ( prodej_id );

CREATE TABLE regiony (
    region_id NUMBER(4) NOT NULL,
    nazev     NVARCHAR2(50) NOT NULL
);

ALTER TABLE regiony ADD CONSTRAINT regiony_pk PRIMARY KEY ( region_id );

CREATE TABLE role (
    role_id NUMBER(2) NOT NULL,
    nazev   NVARCHAR2(30) NOT NULL
);

ALTER TABLE role ADD CONSTRAINT role_pk PRIMARY KEY ( role_id );

CREATE TABLE role_zamestnancu (
    role_id        NUMBER(2) NOT NULL,
    zamestnanec_id NUMBER(5) NOT NULL
);

ALTER TABLE role_zamestnancu ADD CONSTRAINT role_zamestnancu_pk PRIMARY KEY ( role_id,
                                                                              zamestnanec_id );

CREATE TABLE soubory (
    soubor_id        NUMBER(5) NOT NULL,
    nazev_souboru    NVARCHAR2(255) NOT NULL,
    pripona          NVARCHAR2(15) NOT NULL,
    datum_nahrani    TIMESTAMP WITH LOCAL TIME ZONE NOT NULL,
    data             BLOB NOT NULL,
    supermarket_id   NUMBER(5) NOT NULL,
    zamestnanec_id   NUMBER(5) NOT NULL,
    datum_modifikace TIMESTAMP WITH LOCAL TIME ZONE
);

ALTER TABLE soubory ADD CONSTRAINT soubory_pk PRIMARY KEY ( soubor_id );

CREATE TABLE supermarkety (
    supermarket_id NUMBER(5) NOT NULL,
    adresa         NVARCHAR2(100) NOT NULL,
    region_id      NUMBER(4) NOT NULL
);

ALTER TABLE supermarkety ADD CONSTRAINT supermarkety_pk PRIMARY KEY ( supermarket_id );

CREATE TABLE typy_placeni (
    typ_placeni_id NUMBER(3) NOT NULL,
    nazev          NVARCHAR2(30) NOT NULL
);

ALTER TABLE typy_placeni ADD CONSTRAINT typy_placeni_pk PRIMARY KEY ( typ_placeni_id );

CREATE TABLE ulozeni_zbozi (
    kusy             NUMBER(6, 3) NOT NULL,
    misto_ulozeni_id NUMBER(6) NOT NULL,
    supermarket_id   NUMBER(5) NOT NULL,
    zbozi_id         NUMBER(9) NOT NULL
);

ALTER TABLE ulozeni_zbozi
    ADD CONSTRAINT ulozeni_zbozi_pk PRIMARY KEY ( supermarket_id,
                                                  zbozi_id,
                                                  misto_ulozeni_id );

CREATE TABLE zamestnanci (
    zamestnanec_id NUMBER(5) NOT NULL,
    login          VARCHAR2(20) NOT NULL,
    heslo_hash     RAW(32) NOT NULL,
    heslo_salt     RAW(16) NOT NULL,
    jmeno          NVARCHAR2(100) NOT NULL,
    prijmeni       NVARCHAR2(100) NOT NULL,
    datum_nastupu  DATE NOT NULL,
    supermarket_id NUMBER(5),
    manazer_id     NUMBER(5),
    rodne_cislo    VARCHAR2(20)
);

ALTER TABLE zamestnanci ADD CONSTRAINT zamestnanci_pk PRIMARY KEY ( zamestnanec_id );

CREATE TABLE zbozi (
    zbozi_id          NUMBER(9) NOT NULL,
    druh_zbozi_id     NUMBER(4) NOT NULL,
    merna_jednotka_id NUMBER(3) NOT NULL,
    navahu            NUMBER(1) DEFAULT 0 NOT NULL,
    nazev             NVARCHAR2(50) NOT NULL,
    cena              NUMBER(8, 2) NOT NULL,
    carovykod         VARCHAR2(12),
    popis             NVARCHAR2(100),
    dodavatel_id      NUMBER(3) NOT NULL
);

ALTER TABLE zbozi
    ADD CHECK ( navahu IN ( 0, 1 ) );

ALTER TABLE zbozi ADD CONSTRAINT zbozi_pk PRIMARY KEY ( zbozi_id );

ALTER TABLE mista_ulozeni
    ADD CONSTRAINT mista_ulozeni_supermarkety_fk FOREIGN KEY ( supermarket_id )
        REFERENCES supermarkety ( supermarket_id );

ALTER TABLE platba
    ADD CONSTRAINT platba_prodeje_fk FOREIGN KEY ( prodej_id )
        REFERENCES prodeje ( prodej_id );

ALTER TABLE platba
    ADD CONSTRAINT platba_typy_placeni_fk FOREIGN KEY ( typ_placeni_id )
        REFERENCES typy_placeni ( typ_placeni_id );

ALTER TABLE pokladny
    ADD CONSTRAINT pokladny_supermarkety_fk FOREIGN KEY ( supermarket_id )
        REFERENCES supermarkety ( supermarket_id );

ALTER TABLE prodane_zbozi
    ADD CONSTRAINT prodane_zbozi_prodeje_fk FOREIGN KEY ( prodej_id )
        REFERENCES prodeje ( prodej_id );

ALTER TABLE prodane_zbozi
    ADD CONSTRAINT prodane_zbz_pdvn_zbz_fk FOREIGN KEY ( supermarket_id,
                                                         zbozi_id )
        REFERENCES prodavane_zbozi ( supermarket_id,
                                     zbozi_id );

ALTER TABLE prodavane_zbozi
    ADD CONSTRAINT prodavane_zbozi_zbozi_fk FOREIGN KEY ( zbozi_id )
        REFERENCES zbozi ( zbozi_id );

ALTER TABLE prodavane_zbozi
    ADD CONSTRAINT prodavane_zbz_spmkt_fk FOREIGN KEY ( supermarket_id )
        REFERENCES supermarkety ( supermarket_id );

ALTER TABLE prodeje
    ADD CONSTRAINT prodeje_pokladny_fk FOREIGN KEY ( pokladna_id )
        REFERENCES pokladny ( pokladna_id );

ALTER TABLE role_zamestnancu
    ADD CONSTRAINT role_zamestnancu_role_fk FOREIGN KEY ( role_id )
        REFERENCES role ( role_id );

ALTER TABLE role_zamestnancu
    ADD CONSTRAINT role_zmstncu_zamestnanci_fk FOREIGN KEY ( zamestnanec_id )
        REFERENCES zamestnanci ( zamestnanec_id )
            ON DELETE CASCADE;

ALTER TABLE soubory
    ADD CONSTRAINT soubory_supermarkety_fk FOREIGN KEY ( supermarket_id )
        REFERENCES supermarkety ( supermarket_id )
            ON DELETE CASCADE;

ALTER TABLE soubory
    ADD CONSTRAINT soubory_zamestnanci_fk FOREIGN KEY ( zamestnanec_id )
        REFERENCES zamestnanci ( zamestnanec_id );

ALTER TABLE supermarkety
    ADD CONSTRAINT supermarkety_regiony_fk FOREIGN KEY ( region_id )
        REFERENCES regiony ( region_id );

ALTER TABLE ulozeni_zbozi
    ADD CONSTRAINT ulozeni_zbozi_mista_ulozeni_fk FOREIGN KEY ( misto_ulozeni_id )
        REFERENCES mista_ulozeni ( misto_ulozeni_id );

ALTER TABLE ulozeni_zbozi
    ADD CONSTRAINT ulozeni_zbz_pdvn_zbz_fk FOREIGN KEY ( supermarket_id,
                                                         zbozi_id )
        REFERENCES prodavane_zbozi ( supermarket_id,
                                     zbozi_id );

ALTER TABLE zamestnanci
    ADD CONSTRAINT zamestnanci_supermarkety_fk FOREIGN KEY ( supermarket_id )
        REFERENCES supermarkety ( supermarket_id );

ALTER TABLE zamestnanci
    ADD CONSTRAINT zamestnanci_zamestnanci_fk FOREIGN KEY ( manazer_id )
        REFERENCES zamestnanci ( zamestnanec_id );

ALTER TABLE zbozi
    ADD CONSTRAINT zbozi_dodavatele_fk FOREIGN KEY ( dodavatel_id )
        REFERENCES dodavatele ( dodavatel_id );

ALTER TABLE zbozi
    ADD CONSTRAINT zbozi_druhy_zbozi_fk FOREIGN KEY ( druh_zbozi_id )
        REFERENCES druhy_zbozi ( druh_zbozi_id );

ALTER TABLE zbozi
    ADD CONSTRAINT zbozi_merne_jednotky_fk FOREIGN KEY ( merna_jednotka_id )
        REFERENCES merne_jednotky ( merna_jednotka_id );

CREATE OR REPLACE TRIGGER fkntm_prodeje BEFORE
    UPDATE OF pokladna_id ON prodeje
BEGIN
    raise_application_error(-20225, 'Non Transferable FK constraint  on table PRODEJE is violated');
END;
/

CREATE SEQUENCE dodavatele_dodavatel_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER dodavatele_dodavatel_id_trg BEFORE
    INSERT ON dodavatele
    FOR EACH ROW
BEGIN
    :new.dodavatel_id := dodavatele_dodavatel_id_seq.nextval;
END;
/

CREATE SEQUENCE druhy_zbozi_druh_zbozi_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER druhy_zbozi_druh_zbozi_id_trg BEFORE
    INSERT ON druhy_zbozi
    FOR EACH ROW
BEGIN
    :new.druh_zbozi_id := druhy_zbozi_druh_zbozi_id_seq.nextval;
END;
/

CREATE SEQUENCE mista_ulozeni_misto_ulozeni_id START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER mista_ulozeni_misto_ulozeni_id BEFORE
    INSERT ON mista_ulozeni
    FOR EACH ROW
BEGIN
    :new.misto_ulozeni_id := mista_ulozeni_misto_ulozeni_id.nextval;
END;
/

CREATE SEQUENCE pokladny_pokladna_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER pokladny_pokladna_id_trg BEFORE
    INSERT ON pokladny
    FOR EACH ROW
BEGIN
    :new.pokladna_id := pokladny_pokladna_id_seq.nextval;
END;
/

CREATE SEQUENCE prodeje_prodej_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER prodeje_prodej_id_trg BEFORE
    INSERT ON prodeje
    FOR EACH ROW
BEGIN
    :new.prodej_id := prodeje_prodej_id_seq.nextval;
END;
/

CREATE SEQUENCE regiony_region_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER regiony_region_id_trg BEFORE
    INSERT ON regiony
    FOR EACH ROW
BEGIN
    :new.region_id := regiony_region_id_seq.nextval;
END;
/

CREATE SEQUENCE soubory_soubor_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER soubory_soubor_id_trg BEFORE
    INSERT ON soubory
    FOR EACH ROW
BEGIN
    :new.soubor_id := soubory_soubor_id_seq.nextval;
END;
/

CREATE SEQUENCE supermarkety_supermarket_id START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER supermarkety_supermarket_id BEFORE
    INSERT ON supermarkety
    FOR EACH ROW
BEGIN
    :new.supermarket_id := supermarkety_supermarket_id.nextval;
END;
/

CREATE SEQUENCE zamestnanci_zamestnanec_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER zamestnanci_zamestnanec_id_trg BEFORE
    INSERT ON zamestnanci
    FOR EACH ROW
BEGIN
    :new.zamestnanec_id := zamestnanci_zamestnanec_id_seq.nextval;
END;
/

CREATE SEQUENCE zbozi_zbozi_id_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER zbozi_zbozi_id_trg BEFORE
    INSERT ON zbozi
    FOR EACH ROW
BEGIN
    :new.zbozi_id := zbozi_zbozi_id_seq.nextval;
END;
/

insert into druhy_zbozi values (null, 'maso', null);
insert into druhy_zbozi values (null, 'ryby', null);
insert into druhy_zbozi values (null, 'ovoce', null);
insert into druhy_zbozi values (null, 'zelenina', null);
insert into druhy_zbozi values (null, 'pecivo', null);
insert into druhy_zbozi values (null, 'galanterie', null);
insert into druhy_zbozi values (null, 'napoj', null);
insert into druhy_zbozi values (null, 'mlecny vyrobek', null);
insert into druhy_zbozi values (null, 'pripravene potraviny', null);


insert into merne_jednotky values (1, 'kilogram', 'kg'); 
insert into merne_jednotky values (2, 'gram', 'g'); 
insert into merne_jednotky values (3, 'litr', 'l'); 
insert into merne_jednotky values (4, 'mililitr', 'ml');
insert into merne_jednotky values (5, 'kus', 'ks'); 
insert into merne_jednotky values (6, 'metr', 'm'); 


insert into regiony values (null, 'PRAHA');
insert into regiony values (null, 'PARDUBICE');
insert into regiony values (null, 'HRADEC KRALOVE');


insert into typy_placeni values (1, 'hotove');
insert into typy_placeni values (2, 'karta');
insert into typy_placeni values (3, 'kupon');


insert into dodavatele values (null, 'Big Company Inc.');
insert into dodavatele values (null, 'Johnny Redbull');
insert into dodavatele values (null, 'Ceske jidlo');
insert into dodavatele values (null, 'Farming Studio');
insert into dodavatele values (null, 'Milky yoghurt');
insert into dodavatele values (null, 'The Coca-Cola Company');

-- ZBOZI_ID DRUH_ZBOZI_ID MERNA_JEDNOTKA_ID NAVAHU NAZEV CENA HMOTNOST CAROVYKOD POPIS DODAVATEL_ID
insert into zbozi values (1 , 1, 1, 1, 'kuřecí prsa',       160,     '123456780000', null, 3);
insert into zbozi values (2 , 1, 1, 1, 'vepřová plec',      100,     '234231420000', null, 3);
insert into zbozi values (3 , 1, 1, 1, 'hovězí steak',      30,    '567832880000', null, 4);
insert into zbozi values (4 , 2, 2, 1, 'losos',             45,    '428359210000', null, 1);
insert into zbozi values (5 , 2, 2, 1, 'kapr',              20,    '159158290000', null, 2);
insert into zbozi values (6 , 4, 1, 1, 'brambory',          20,      '518325810000', null, 4);
insert into zbozi values (7 , 4, 5, 0, 'okurka',            15,     '591358190000', null, 2);
insert into zbozi values (8 , 3, 1, 1, 'jablko',            25,      '485129590000', null, 4);
insert into zbozi values (9 , 3, 1, 1, 'citron',            15,      '519124990000', null, 4);
insert into zbozi values (10, 5, 5, 0, 'rohlík',            3,    '412941050000', null, 2);
insert into zbozi values (11, 5, 5, 0, 'párek v rohlíku',   15,   '592159120000', null, 2);
insert into zbozi values (12, 6, 6, 1, 'nýt',               30,   '591293510000', null, 5);
insert into zbozi values (13, 6, 5, 0, 'měřítko',           25,   '128512940000', null, 5);
insert into zbozi values (14, 7, 5, 0, 'voda 1l',              10,   '581091540000', null, 6);
insert into zbozi values (15, 7, 5, 0, 'pepsi 2l',             30,   '581259540000', null, 6);
insert into zbozi values (16, 7, 5, 0, 'jablečný džus 1l',     25,   '481259210000', null, 2);
insert into zbozi values (17, 8, 5, 0, 'jogurt 250g',            20,   '581249580000', null, 5);
insert into zbozi values (18, 8, 5, 0, 'smetana 250g',           25,   '581519520000', null, 5);
insert into zbozi values (19, 9, 5, 0, 'kuřecí nugety 500g',     45,   '581259150000', null, 1);
insert into zbozi values (20, 9, 5, 0, 'mražená pizza 350g',     50,   '586195820000', null, 1);

insert into zbozi values (21, 4, 1, 1, 'červená řepa',      12, '581259810000', null, 4);
insert into zbozi values (22, 4, 1, 1, 'zelí',              18, '581295820000', null, 4);
insert into zbozi values (23, 5, 5, 0, 'rohlík se semínky', 5, '581259520000', null, 2);
insert into zbozi values (24, 8, 5, 0, 'jogurt 10% 250g',           30,  '581259820000', null, 5);
insert into zbozi values (25, 7, 5, 0, 'čaj 0.5l',               30,'581259120000', null, 6);
insert into zbozi values (26, 8, 5, 0, 'tvaroh',            30, '581259580000', null, 5);
insert into zbozi values (27, 9, 5, 0, 'pommes frites',     35,'581259820000', null, 1);
insert into zbozi values (28, 9, 5, 0, 'hranolky',          40,'581259820000', null, 1);
insert into zbozi values (29, 6, 5, 0, 'špejle',           5,    '581259520000', null, 5);
insert into zbozi values (30, 5, 5, 0, 'žemle',            3,   '581259520000', null, 2);
insert into zbozi values (31, 1, 1, 1, 'kuřecí stehno',      120, '581259810000', null, 3);
insert into zbozi values (32, 1, 1, 1, 'kuřecí křídla',      150, '581295820000', null, 3);
insert into zbozi values (33, 2, 2, 1, 'pstruh',             60, '581259520000', null, 1);
insert into zbozi values (34, 2, 2, 1, 'kapří filé',         80, '581259820000', null, 2);
insert into zbozi values (35, 4, 1, 1, 'mrkev',              10, '581259520000', null, 4);
insert into zbozi values (36, 4, 1, 1, 'cibule',             8, '581259820000', null, 4);
insert into zbozi values (37, 4, 1, 1, 'česnek',             15, '581259120000', null, 4);
insert into zbozi values (38, 4, 1, 1, 'zelí kysané',        20, '581259520000', null, 4);
insert into zbozi values (39, 5, 5, 0, 'bageta',             25,   '581259520000', null, 2);
insert into zbozi values (40, 5, 5, 0, 'houska',             5,    '581259520000', null, 2);
insert into zbozi values (41, 6, 5, 0, 'hrábě',              30, '581259820000', null, 5);
insert into zbozi values (42, 6, 5, 0, 'vidle',              25, '581259520000', null, 5);
insert into zbozi values (43, 7, 5, 0, 'čaj zelený',         50, '581259820000', null, 6);
insert into zbozi values (44, 7, 5, 0, 'čaj černý',          40, '581259520000', null, 6);
insert into zbozi values (45, 7, 5, 0, 'limonáda',           30, '581259520000', null, 6);
insert into zbozi values (46, 7, 5, 0, 'cola',               35, '581259520000', null, 6);
insert into zbozi values (47, 8, 5, 0, 'tvaroh 0%',          25, '581259120000', null, 5);
insert into zbozi values (48, 8, 5, 0, 'tvaroh 10%',         30, '581259820000', null, 5);
insert into zbozi values (49, 8, 5, 0, 'tvaroh 20%',         35, '581259520000', null, 5);
insert into zbozi values (50, 9, 5, 0, 'kurčecí nuggets',    50, '581259820000', null, 1);
insert into zbozi values (51, 9, 5, 0, 'hovězí plátek',       65, '581259820000', null, 1);
insert into zbozi values (52, 1, 1, 1, 'kuřecí klobása',      80,  '581259520000', null, 3);
insert into zbozi values (53, 1, 1, 1, 'kuřecí pařátka',      70,  '581259820000', null, 3);
insert into zbozi values (54, 2, 2, 1, 'pangasius',           50,  '581259520000', null, 1);
insert into zbozi values (55, 2, 2, 1, 'lososové file',       75,  '581259820000', null, 1);
insert into zbozi values (56, 4, 1, 1, 'petržel',             10,  '581259520000', null, 4);
insert into zbozi values (57, 4, 1, 1, 'mrkev krájená',       12,  '581259820000', null, 4);
insert into zbozi values (58, 3, 1, 1, 'banán',               30,  '581259120000', null, 4);
insert into zbozi values (59, 3, 1, 1, 'pomeranč',            25,  '581259520000', null, 4);
insert into zbozi values (60, 5, 5, 0, 'buchta',              15,  '581259520000', null, 2);
insert into zbozi values (61, 5, 5, 0, 'koláč',               20,  '581259520000', null, 2);
insert into zbozi values (62, 6, 5, 0, 'hrnec',               35,  '581259820000', null, 5);

insert into zbozi values (63, 6, 5, 0, 'kastrůlek',           30,  '581259520000', null, 5);
insert into zbozi values (64, 7, 5, 0, 'minerální voda',      20,  '581259520000', null, 6);
insert into zbozi values (65, 7, 5, 0, 'limonádový sirup',    25,  '581259520000', null, 6);
insert into zbozi values (66, 7, 5, 0, 'černý čaj',           30,  '581259520000', null, 6);
insert into zbozi values (67, 7, 5, 0, 'zelený čaj',          35,  '581259520000', null, 6);
insert into zbozi values (68, 8, 5, 0, 'smetana 10%',         30,  '581259820000', null, 5);
insert into zbozi values (69, 8, 5, 0, 'smetana 20%',         35,  '581259520000', null, 5);
insert into zbozi values (70, 9, 5, 0, 'rostbif',             55,  '581259820000', null, 1);
insert into zbozi values (71, 9, 5, 0, 'řízek',                40,  '581259820000', null, 1);
insert into zbozi values (72, 1, 1, 1, 'kuřecí stehenní řízek', 80,  '581259520000', null, 3);
insert into zbozi values (73, 1, 1, 1, 'kuřecí játra',         60,  '581259820000', null, 3);
insert into zbozi values (74, 2, 2, 1, 'treska',               70,  '581259520000', null, 1);
insert into zbozi values (75, 2, 2, 1, 'čerstvý kapří file',   90,  '581259820000', null, 1);
insert into zbozi values (76, 4, 1, 1, 'kedlubna',             15,  '581259520000', null, 4);
insert into zbozi values (77, 4, 1, 1, 'petržel kořenová',     18,  '581259820000', null, 4);
insert into zbozi values (78, 3, 1, 1, 'kiwi',                 35,  '581259120000', null, 4);
insert into zbozi values (79, 3, 1, 1, 'mandarinka',           30,  '581259520000', null, 4);
insert into zbozi values (80, 6, 5, 0, 'pokladnicek',          12,  '581259520000', null, 2);
insert into zbozi values (81, 6, 5, 0, 'košíček',              10,  '581259520000', null, 2);
insert into zbozi values (82, 6, 5, 0, 'lžíce',                25,  '581259820000', null, 5);
insert into zbozi values (83, 6, 5, 0, 'vidlička',             20,  '581259520000', null, 5);
insert into zbozi values (84, 7, 5, 0, 'čaj z bylin',          50,  '581259520000', null, 6);
insert into zbozi values (85, 7, 5, 0, 'malinová limonáda',    35,  '581259520000', null, 6);
insert into zbozi values (86, 7, 5, 0, 'pomerančový džus',     40,  '581259520000', null, 6);
insert into zbozi values (87, 7, 5, 0, 'voda s příchutí',      30,  '581259520000', null, 6);
insert into zbozi values (88, 8, 5, 0, 'kefír',                20,  '581259820000', null, 5);
insert into zbozi values (89, 8, 5, 0, 'jogurt 0%',            25,  '581259520000', null, 5);
insert into zbozi values (90, 7, 5, 0, 'fanta',             30,     '581259540000', null, 6);

insert into zbozi values (91, 9, 5, 0, 'vepřové maso',          60,   '581259820000', null, 1);
insert into zbozi values (92, 1, 1, 1, 'kuřecí prsa na řízky',  70, '581259520000', null, 3);
insert into zbozi values (93, 1, 1, 1, 'kuřecí křídla pikantní', 85, '581259820000', null, 3);
insert into zbozi values (94, 2, 2, 1, 'obalené tresčí file',   75, '581259520000', null, 1);
insert into zbozi values (95, 2, 2, 1, 'lososový steak',        100, '581259820000', null, 1);
insert into zbozi values (96, 4, 1, 1, 'ředkvička',             8, '581259520000', null, 4);
insert into zbozi values (97, 4, 1, 1, 'kedlubna červená',      12, '581259820000', null, 4);
insert into zbozi values (98, 3, 1, 1, 'jablko zelené',        25, '581259120000', null, 4);
insert into zbozi values (99, 3, 1, 1, 'granátové jablko',     35, '581259520000', null, 4);
insert into zbozi values (100, 6, 5, 0, 'plakát',               15,    '581259520000', null, 2);
insert into zbozi values (101, 6, 5, 0, 'časopis',              5,    '581259520000', null, 2);
insert into zbozi values (102, 6, 5, 0, 'lžička',               10,    '581259820000', null, 5);
insert into zbozi values (103, 6, 5, 0, 'nůž',                  15,    '581259520000', null, 5);
insert into zbozi values (104, 7, 5, 0, 'kakao',                40,    '581259520000', null, 6);
insert into zbozi values (105, 7, 5, 0, 'jahodový džus',        35,    '581259520000', null, 6);
insert into zbozi values (106, 7, 5, 0, 'černý čaj s bergamotem', 30, '581259520000', null, 6);
insert into zbozi values (107, 7, 5, 0, 'limetkový džus',       25,     '581259520000', null, 6);
insert into zbozi values (108, 8, 5, 0, 'jogurt 20%',            30, '581259820000', null, 5);
insert into zbozi values (109, 8, 5, 0, 'jogurt 30%',            35, '581259520000', null, 5);
insert into zbozi values (110, 8, 5, 0, 'jogurt s příchutí',     40, '581259820000', null, 5);


insert into supermarkety values (null, 'ulice Kloboucnicka 1735/24', 1);
insert into supermarkety values (null, 'nam. Na Balabence', 1);
insert into supermarkety values (null, 'Vysehradska 421/21', 1);
insert into supermarkety values (null, 'Strossova', 2);
insert into supermarkety values (null, 'Palackeho tr. 1948', 2);
insert into supermarkety values (null, 'V Kopecku 80', 3);


insert into pokladny values(null, 1, 'POKLADNA 1', 'P1', null);
insert into pokladny values(null, 1, 'POKLADNA 2', 'P2', null);
insert into pokladny values(null, 1, 'POKLADNA 3', 'P3', null);
insert into pokladny values(null, 2, 'POKLADNA 1', 'P1', null);
insert into pokladny values(null, 2, 'POKLADNA 2', 'P2', null);
insert into pokladny values(null, 2, 'POKLADNA 3', 'P3', null);
insert into pokladny values(null, 3, 'POKLADNA 1', 'P1', null);
insert into pokladny values(null, 3, 'POKLADNA 2', 'P2', null);
insert into pokladny values(null, 3, 'POKLADNA 3', 'P3', null);
insert into pokladny values(null, 4, 'POKLADNA 1', 'P1', null);
insert into pokladny values(null, 4, 'POKLADNA 2', 'P2', null);
insert into pokladny values(null, 5, 'POKLADNA 1', 'P1', null);
insert into pokladny values(null, 5, 'POKLADNA 2', 'P2', null);
insert into pokladny values(null, 6, 'POKLADNA 1', 'P1', null);


insert into prodavane_zbozi values(1 , 1, 1);
insert into prodavane_zbozi values(2 , 1, 1);
insert into prodavane_zbozi values(3 , 1, 1);
insert into prodavane_zbozi values(4 , 1, 1);
insert into prodavane_zbozi values(5 , 1, 1);
insert into prodavane_zbozi values(6 , 1, 1);
insert into prodavane_zbozi values(7 , 1, 1);
insert into prodavane_zbozi values(8 , 1, 1);
insert into prodavane_zbozi values(9 , 1, 1);
insert into prodavane_zbozi values(10, 1, 1);
insert into prodavane_zbozi values(11, 1, 1);
insert into prodavane_zbozi values(12, 1, 1);
insert into prodavane_zbozi values(13, 1, 1);
insert into prodavane_zbozi values(14, 1, 1);
insert into prodavane_zbozi values(15, 1, 1);
insert into prodavane_zbozi values(16, 1, 1);
insert into prodavane_zbozi values(17, 1, 1);
insert into prodavane_zbozi values(18, 1, 1);
insert into prodavane_zbozi values(19, 1, 1);
insert into prodavane_zbozi values(20, 1, 1);
insert into prodavane_zbozi values(21, 1, 1);
insert into prodavane_zbozi values(22, 1, 1);
insert into prodavane_zbozi values(23, 1, 1);
insert into prodavane_zbozi values(24, 1, 1);
insert into prodavane_zbozi values(25, 1, 1);
insert into prodavane_zbozi values(26, 1, 1);
insert into prodavane_zbozi values(27, 1, 1);
insert into prodavane_zbozi values(28, 1, 1);
insert into prodavane_zbozi values(29, 1, 1);
insert into prodavane_zbozi values(30, 1, 1);
insert into prodavane_zbozi values(31, 1, 1);
insert into prodavane_zbozi values(32, 1, 1);
insert into prodavane_zbozi values(33, 1, 1);
insert into prodavane_zbozi values(34, 1, 1);
insert into prodavane_zbozi values(35, 1, 1);
insert into prodavane_zbozi values(36, 1, 1);
insert into prodavane_zbozi values(37, 1, 1);
insert into prodavane_zbozi values(38, 1, 1);
insert into prodavane_zbozi values(39, 1, 1);
insert into prodavane_zbozi values(40, 1, 1);
insert into prodavane_zbozi values(41, 1, 1);
insert into prodavane_zbozi values(42, 1, 1);
insert into prodavane_zbozi values(43, 1, 1);
insert into prodavane_zbozi values(44, 1, 1);
insert into prodavane_zbozi values(45, 1, 1);
insert into prodavane_zbozi values(46, 1, 1);
insert into prodavane_zbozi values(47, 1, 1);
insert into prodavane_zbozi values(48, 1, 1);
insert into prodavane_zbozi values(49, 1, 1);
insert into prodavane_zbozi values(50, 1, 1);
insert into prodavane_zbozi values(51, 1, 1);
insert into prodavane_zbozi values(52, 1, 1);
insert into prodavane_zbozi values(53, 1, 1);
insert into prodavane_zbozi values(54, 1, 1);
insert into prodavane_zbozi values(55, 1, 1);
insert into prodavane_zbozi values(56, 1, 1);
insert into prodavane_zbozi values(57, 1, 1);
insert into prodavane_zbozi values(58, 1, 1);
insert into prodavane_zbozi values(59, 1, 1);
insert into prodavane_zbozi values(60, 1, 1);
insert into prodavane_zbozi values(61, 1, 1);
insert into prodavane_zbozi values(62, 1, 1);
insert into prodavane_zbozi values(63, 1, 1);
insert into prodavane_zbozi values(64, 1, 1);
insert into prodavane_zbozi values(65, 1, 1);
insert into prodavane_zbozi values(66, 1, 1);
insert into prodavane_zbozi values(67, 1, 1);
insert into prodavane_zbozi values(68, 1, 1);
insert into prodavane_zbozi values(69, 1, 1);
insert into prodavane_zbozi values(70, 1, 1);
insert into prodavane_zbozi values(71, 1, 1);
insert into prodavane_zbozi values(72, 1, 1);
insert into prodavane_zbozi values(73, 1, 1);
insert into prodavane_zbozi values(74, 1, 1);
insert into prodavane_zbozi values(75, 1, 1);
insert into prodavane_zbozi values(76, 1, 1);
insert into prodavane_zbozi values(77, 1, 1);
insert into prodavane_zbozi values(78, 1, 1);
insert into prodavane_zbozi values(79, 1, 1);
insert into prodavane_zbozi values(80, 1, 1);
insert into prodavane_zbozi values(81, 1, 1);
insert into prodavane_zbozi values(82, 1, 1);
insert into prodavane_zbozi values(83, 1, 1);
insert into prodavane_zbozi values(84, 1, 1);
insert into prodavane_zbozi values(85, 1, 1);
insert into prodavane_zbozi values(86, 1, 1);
insert into prodavane_zbozi values(87, 1, 1);
insert into prodavane_zbozi values(88, 1, 1);
insert into prodavane_zbozi values(89, 1, 1);
insert into prodavane_zbozi values(90, 1, 1);
insert into prodavane_zbozi values(91, 1, 1);
insert into prodavane_zbozi values(92, 1, 1);
insert into prodavane_zbozi values(93, 1, 1);
insert into prodavane_zbozi values(94, 1, 1);
insert into prodavane_zbozi values(95, 1, 1);
insert into prodavane_zbozi values(96, 1, 1);
insert into prodavane_zbozi values(97, 1, 1);
insert into prodavane_zbozi values(98, 1, 1);
insert into prodavane_zbozi values(99, 1, 1);
insert into prodavane_zbozi values(100, 1, 1);
insert into prodavane_zbozi values(101, 1, 1);
insert into prodavane_zbozi values(102, 1, 1);
insert into prodavane_zbozi values(103, 1, 1);
insert into prodavane_zbozi values(104, 1, 1);
insert into prodavane_zbozi values(105, 1, 1);
insert into prodavane_zbozi values(106, 1, 1);
insert into prodavane_zbozi values(107, 1, 1);
insert into prodavane_zbozi values(108, 1, 1);
insert into prodavane_zbozi values(109, 1, 1);
insert into prodavane_zbozi values(110, 1, 1);



insert into prodavane_zbozi values(1 , 2, 1);
insert into prodavane_zbozi values(2 , 2, 1);
insert into prodavane_zbozi values(3 , 2, 1);
insert into prodavane_zbozi values(4 , 2, 1);
insert into prodavane_zbozi values(5 , 2, 1);
insert into prodavane_zbozi values(6 , 2, 1);
insert into prodavane_zbozi values(7 , 2, 1);
insert into prodavane_zbozi values(8 , 2, 1);
insert into prodavane_zbozi values(10, 2, 1);
insert into prodavane_zbozi values(11, 2, 1);
insert into prodavane_zbozi values(12, 2, 1);
insert into prodavane_zbozi values(13, 2, 1);
insert into prodavane_zbozi values(14, 2, 1);
insert into prodavane_zbozi values(16, 2, 1);
insert into prodavane_zbozi values(17, 2, 1);
insert into prodavane_zbozi values(18, 2, 1);
insert into prodavane_zbozi values(19, 2, 1);
insert into prodavane_zbozi values(20, 2, 1);
insert into prodavane_zbozi values(1 , 3, 1);
insert into prodavane_zbozi values(2 , 3, 1);
insert into prodavane_zbozi values(3 , 3, 1);
insert into prodavane_zbozi values(4 , 3, 1);
insert into prodavane_zbozi values(5 , 3, 1);
insert into prodavane_zbozi values(6 , 3, 1);
insert into prodavane_zbozi values(7 , 3, 1);
insert into prodavane_zbozi values(8 , 3, 1);
insert into prodavane_zbozi values(9 , 3, 1);
insert into prodavane_zbozi values(10, 3, 1);
insert into prodavane_zbozi values(11, 3, 1);
insert into prodavane_zbozi values(12, 3, 1);
insert into prodavane_zbozi values(13, 3, 1);
insert into prodavane_zbozi values(14, 3, 1);
insert into prodavane_zbozi values(15, 3, 1);
insert into prodavane_zbozi values(16, 3, 1);
insert into prodavane_zbozi values(17, 3, 1);
insert into prodavane_zbozi values(18, 3, 1);
insert into prodavane_zbozi values(19, 3, 1);
insert into prodavane_zbozi values(20, 3, 1);
insert into prodavane_zbozi values(2 , 4, 1);
insert into prodavane_zbozi values(3 , 4, 1);
insert into prodavane_zbozi values(4 , 4, 1);
insert into prodavane_zbozi values(5 , 4, 1);
insert into prodavane_zbozi values(7 , 4, 1);
insert into prodavane_zbozi values(9 , 4, 1);
insert into prodavane_zbozi values(10, 4, 1);
insert into prodavane_zbozi values(11, 4, 1);
insert into prodavane_zbozi values(12, 4, 1);
insert into prodavane_zbozi values(13, 4, 1);
insert into prodavane_zbozi values(14, 4, 1);
insert into prodavane_zbozi values(15, 4, 1);
insert into prodavane_zbozi values(16, 4, 1);
insert into prodavane_zbozi values(17, 4, 1);
insert into prodavane_zbozi values(18, 4, 1);
insert into prodavane_zbozi values(19, 4, 1);
insert into prodavane_zbozi values(20, 4, 1);
insert into prodavane_zbozi values(1 , 5, 1);
insert into prodavane_zbozi values(2 , 5, 1);
insert into prodavane_zbozi values(4 , 5, 1);
insert into prodavane_zbozi values(5 , 5, 1);
insert into prodavane_zbozi values(7 , 5, 1);
insert into prodavane_zbozi values(8 , 5, 1);
insert into prodavane_zbozi values(9 , 5, 1);
insert into prodavane_zbozi values(10, 5, 1);
insert into prodavane_zbozi values(11, 5, 1);
insert into prodavane_zbozi values(12, 5, 1);
insert into prodavane_zbozi values(13, 5, 1);
insert into prodavane_zbozi values(14, 5, 1);
insert into prodavane_zbozi values(15, 5, 1);
insert into prodavane_zbozi values(16, 5, 1);
insert into prodavane_zbozi values(17, 5, 1);
insert into prodavane_zbozi values(18, 5, 1);
insert into prodavane_zbozi values(19, 5, 1);
insert into prodavane_zbozi values(20, 5, 1);
insert into prodavane_zbozi values(1 , 6, 1);
insert into prodavane_zbozi values(2 , 6, 1);
insert into prodavane_zbozi values(4 , 6, 1);
insert into prodavane_zbozi values(6 , 6, 1);
insert into prodavane_zbozi values(7 , 6, 1);
insert into prodavane_zbozi values(8 , 6, 1);
insert into prodavane_zbozi values(9 , 6, 1);
insert into prodavane_zbozi values(11, 6, 1);
insert into prodavane_zbozi values(12, 6, 1);
insert into prodavane_zbozi values(13, 6, 1);
insert into prodavane_zbozi values(14, 6, 1);
insert into prodavane_zbozi values(15, 6, 1);
insert into prodavane_zbozi values(16, 6, 1);
insert into prodavane_zbozi values(17, 6, 1);
insert into prodavane_zbozi values(19, 6, 1);


insert into mista_ulozeni values(null , 'S15', null, 1, 'SKLAD');
insert into mista_ulozeni values(null, 'S2', null, 1, 'SKLAD');
insert into mista_ulozeni values(null, 'P1', null, 1, 'PULT');
insert into mista_ulozeni values(null, 'P2', null, 1, 'PULT');
insert into mista_ulozeni values(null, 'P3', null, 1, 'PULT');
insert into mista_ulozeni values(null, 'P4', null, 1, 'PULT');
insert into mista_ulozeni values(null, 'P5', null, 1, 'PULT');
insert into mista_ulozeni values(null, 'S1', null, 2, 'SKLAD');
insert into mista_ulozeni values(null, 'P1', null, 2, 'PULT');
insert into mista_ulozeni values(null, 'P2', null, 2, 'PULT');
insert into mista_ulozeni values(null, 'P3', null, 2, 'PULT');
insert into mista_ulozeni values(null, 'P4', null, 2, 'PULT');
insert into mista_ulozeni values(null, 'P5', null, 2, 'PULT');
insert into mista_ulozeni values(null, 'S1', null, 3, 'SKLAD');
insert into mista_ulozeni values(null, 'P1', null, 3, 'PULT');
insert into mista_ulozeni values(null, 'P2', null, 3, 'PULT');
insert into mista_ulozeni values(null, 'P3', null, 3, 'PULT');
insert into mista_ulozeni values(null, 'P4', null, 3, 'PULT');
insert into mista_ulozeni values(null, 'P5', null, 3, 'PULT');
insert into mista_ulozeni values(null, 'S1', null, 4, 'SKLAD');
insert into mista_ulozeni values(null, 'P1', null, 4, 'PULT');
insert into mista_ulozeni values(null, 'P2', null, 4, 'PULT');
insert into mista_ulozeni values(null, 'P3', null, 4, 'PULT');
insert into mista_ulozeni values(null, 'P4', null, 4, 'PULT');
insert into mista_ulozeni values(null, 'P5', null, 4, 'PULT');
insert into mista_ulozeni values(null, 'S1', null, 5, 'SKLAD');
insert into mista_ulozeni values(null, 'P1', null, 5, 'PULT');
insert into mista_ulozeni values(null, 'P3', null, 5, 'PULT');
insert into mista_ulozeni values(null, 'P4', null, 5, 'PULT');
insert into mista_ulozeni values(null, 'P5', null, 5, 'PULT');
insert into mista_ulozeni values(null, 'S1', null, 6, 'SKLAD');
insert into mista_ulozeni values(null, 'P1', null, 6, 'PULT');
insert into mista_ulozeni values(null, 'P3', null, 6, 'PULT');
insert into mista_ulozeni values(null, 'P4', null, 6, 'PULT');
insert into mista_ulozeni values(null, 'P5', null, 6, 'PULT');
insert into mista_ulozeni values(null, 'OS1', null, 3, 'SKLAD');
insert into mista_ulozeni values(null, 'MS1', null, 3, 'JINE');


insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 09:40:15.62', 'DD-MM-YYYY HH24:MI:SS.FF'), 1);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 10:13:13.35', 'DD-MM-YYYY HH24:MI:SS.FF'), 1);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 11:53:42.62', 'DD-MM-YYYY HH24:MI:SS.FF'), 2);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 13:14:13.34', 'DD-MM-YYYY HH24:MI:SS.FF'), 2);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 14:12:12.73', 'DD-MM-YYYY HH24:MI:SS.FF'), 3);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 15:42:42.47', 'DD-MM-YYYY HH24:MI:SS.FF'), 3);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 16:24:23.74', 'DD-MM-YYYY HH24:MI:SS.FF'), 4);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 16:45:52.73', 'DD-MM-YYYY HH24:MI:SS.FF'), 4);
insert into prodeje values(null, TO_TIMESTAMP('10-12-2022 17:13:23.67', 'DD-MM-YYYY HH24:MI:SS.FF'), 5);
insert into prodeje values(null, TO_TIMESTAMP('11-12-2022 09:34:42.23', 'DD-MM-YYYY HH24:MI:SS.FF'), 5);
insert into prodeje values(null, TO_TIMESTAMP('11-12-2022 10:13:32.32', 'DD-MM-YYYY HH24:MI:SS.FF'), 6);
insert into prodeje values(null, TO_TIMESTAMP('11-12-2022 12:54:34.63', 'DD-MM-YYYY HH24:MI:SS.FF'), 7);
insert into prodeje values(null, TO_TIMESTAMP('11-12-2022 13:34:42.62', 'DD-MM-YYYY HH24:MI:SS.FF'), 8);
insert into prodeje values(null, TO_TIMESTAMP('11-12-2022 13:52:24.36', 'DD-MM-YYYY HH24:MI:SS.FF'), 9);
insert into prodeje values(null, TO_TIMESTAMP('11-12-2022 15:12:32.32', 'DD-MM-YYYY HH24:MI:SS.FF'), 9);
insert into prodeje values(null, TO_TIMESTAMP('12-12-2022 10:14:34.36', 'DD-MM-YYYY HH24:MI:SS.FF'), 10);
insert into prodeje values(null, TO_TIMESTAMP('12-12-2022 12:42:42.64', 'DD-MM-YYYY HH24:MI:SS.FF'), 11);
insert into prodeje values(null, TO_TIMESTAMP('12-12-2022 13:12:13.41', 'DD-MM-YYYY HH24:MI:SS.FF'), 12);
insert into prodeje values(null, TO_TIMESTAMP('13-12-2022 12:11:42.23', 'DD-MM-YYYY HH24:MI:SS.FF'), 13);
insert into prodeje values(null, TO_TIMESTAMP('13-12-2022 14:41:51.13', 'DD-MM-YYYY HH24:MI:SS.FF'), 14);




insert into ulozeni_zbozi values (62, 1, 1, 1);
insert into ulozeni_zbozi values (67, 1, 1, 2);
insert into ulozeni_zbozi values (13, 1, 1, 3);
insert into ulozeni_zbozi values (23, 1, 1, 4);
insert into ulozeni_zbozi values (66, 1, 1, 5);
insert into ulozeni_zbozi values (33, 1, 1, 6);
insert into ulozeni_zbozi values (11, 1, 1, 7);
insert into ulozeni_zbozi values (47, 1, 1, 8);
insert into ulozeni_zbozi values (20, 1, 1, 9);
insert into ulozeni_zbozi values (53, 1, 1, 10);
insert into ulozeni_zbozi values (57, 1, 1, 11);
insert into ulozeni_zbozi values (12, 1, 1, 12);
insert into ulozeni_zbozi values (49, 1, 1, 13);
insert into ulozeni_zbozi values (19, 1, 1, 14);
insert into ulozeni_zbozi values (27, 1, 1, 15);
insert into ulozeni_zbozi values (19, 1, 1, 16);
insert into ulozeni_zbozi values (38, 1, 1, 17);
insert into ulozeni_zbozi values (38, 1, 1, 18);
insert into ulozeni_zbozi values (43, 1, 1, 19);
insert into ulozeni_zbozi values (43, 1, 1, 20);
insert into ulozeni_zbozi values (22, 2, 1, 1);
insert into ulozeni_zbozi values (52, 2, 1, 2);
insert into ulozeni_zbozi values (36, 2, 1, 3);
insert into ulozeni_zbozi values (17, 2, 1, 4);
insert into ulozeni_zbozi values (65, 2, 1, 5);
insert into ulozeni_zbozi values (65, 2, 1, 6);
insert into ulozeni_zbozi values (14, 2, 1, 7);
insert into ulozeni_zbozi values (54, 2, 1, 8);
insert into ulozeni_zbozi values (43, 2, 1, 9);
insert into ulozeni_zbozi values (45, 2, 1, 10);
insert into ulozeni_zbozi values (21, 2, 1, 11);
insert into ulozeni_zbozi values (63, 2, 1, 12);
insert into ulozeni_zbozi values (66, 2, 1, 13);
insert into ulozeni_zbozi values (35, 2, 1, 14);
insert into ulozeni_zbozi values (32, 2, 1, 15);
insert into ulozeni_zbozi values (35, 2, 1, 16);
insert into ulozeni_zbozi values (56, 2, 1, 17);
insert into ulozeni_zbozi values (40, 2, 1, 18);
insert into ulozeni_zbozi values (43, 2, 1, 19);
insert into ulozeni_zbozi values (44, 2, 1, 20);
insert into ulozeni_zbozi values (47, 8, 2, 1);
insert into ulozeni_zbozi values (51, 8, 2, 2);
insert into ulozeni_zbozi values (31, 8, 2, 3);
insert into ulozeni_zbozi values (34, 8, 2, 4);
insert into ulozeni_zbozi values (49, 8, 2, 5);
insert into ulozeni_zbozi values (66, 8, 2, 6);
insert into ulozeni_zbozi values (16, 8, 2, 7);
insert into ulozeni_zbozi values (30, 8, 2, 8);
insert into ulozeni_zbozi values (18, 8, 2, 10);
insert into ulozeni_zbozi values (31, 8, 2, 11);
insert into ulozeni_zbozi values (46, 8, 2, 12);
insert into ulozeni_zbozi values (20, 8, 2, 13);
insert into ulozeni_zbozi values (41, 8, 2, 14);
insert into ulozeni_zbozi values (51, 8, 2, 16);
insert into ulozeni_zbozi values (50, 8, 2, 17);
insert into ulozeni_zbozi values (42, 8, 2, 18);
insert into ulozeni_zbozi values (36, 8, 2, 19);
insert into ulozeni_zbozi values (64, 8, 2, 20);
insert into ulozeni_zbozi values (52, 14, 3, 1);
insert into ulozeni_zbozi values (15, 14, 3, 2);
insert into ulozeni_zbozi values (63, 14, 3, 3);
insert into ulozeni_zbozi values (10, 14, 3, 4);
insert into ulozeni_zbozi values (68, 14, 3, 5);
insert into ulozeni_zbozi values (64, 14, 3, 6);
insert into ulozeni_zbozi values (10, 14, 3, 7);
insert into ulozeni_zbozi values (69, 14, 3, 8);
insert into ulozeni_zbozi values (28, 14, 3, 9);
insert into ulozeni_zbozi values (38, 14, 3, 10);
insert into ulozeni_zbozi values (57, 14, 3, 11);
insert into ulozeni_zbozi values (39, 14, 3, 12);
insert into ulozeni_zbozi values (60, 14, 3, 13);
insert into ulozeni_zbozi values (24, 14, 3, 14);
insert into ulozeni_zbozi values (47, 14, 3, 15);
insert into ulozeni_zbozi values (60, 14, 3, 16);
insert into ulozeni_zbozi values (69, 14, 3, 17);
insert into ulozeni_zbozi values (57, 14, 3, 18);
insert into ulozeni_zbozi values (53, 14, 3, 19);
insert into ulozeni_zbozi values (49, 14, 3, 20);
insert into ulozeni_zbozi values (43, 20, 4, 2);
insert into ulozeni_zbozi values (46, 20, 4, 3);
insert into ulozeni_zbozi values (54, 20, 4, 4);
insert into ulozeni_zbozi values (59, 20, 4, 5);
insert into ulozeni_zbozi values (48, 20, 4, 7);
insert into ulozeni_zbozi values (11, 20, 4, 9);
insert into ulozeni_zbozi values (42, 20, 4, 10);
insert into ulozeni_zbozi values (18, 20, 4, 11);
insert into ulozeni_zbozi values (63, 20, 4, 12);
insert into ulozeni_zbozi values (53, 20, 4, 13);
insert into ulozeni_zbozi values (54, 20, 4, 14);
insert into ulozeni_zbozi values (27, 20, 4, 15);
insert into ulozeni_zbozi values (57, 20, 4, 16);
insert into ulozeni_zbozi values (35, 20, 4, 17);
insert into ulozeni_zbozi values (23, 20, 4, 18);
insert into ulozeni_zbozi values (50, 20, 4, 19);
insert into ulozeni_zbozi values (14, 20, 4, 20);
insert into ulozeni_zbozi values (11, 26, 5, 1);
insert into ulozeni_zbozi values (56, 26, 5, 2);
insert into ulozeni_zbozi values (52, 26, 5, 4);
insert into ulozeni_zbozi values (47, 26, 5, 5);
insert into ulozeni_zbozi values (37, 26, 5, 7);
insert into ulozeni_zbozi values (45, 26, 5, 8);
insert into ulozeni_zbozi values (11, 26, 5, 9);
insert into ulozeni_zbozi values (15, 26, 5, 10);
insert into ulozeni_zbozi values (49, 26, 5, 11);
insert into ulozeni_zbozi values (62, 26, 5, 12);
insert into ulozeni_zbozi values (21, 26, 5, 13);
insert into ulozeni_zbozi values (47, 26, 5, 14);
insert into ulozeni_zbozi values (11, 26, 5, 15);
insert into ulozeni_zbozi values (17, 26, 5, 16);
insert into ulozeni_zbozi values (63, 26, 5, 17);
insert into ulozeni_zbozi values (37, 26, 5, 18);
insert into ulozeni_zbozi values (55, 26, 5, 19);
insert into ulozeni_zbozi values (51, 26, 5, 20);
insert into ulozeni_zbozi values (34, 31, 6, 1);
insert into ulozeni_zbozi values (25, 31, 6, 2);
insert into ulozeni_zbozi values (23, 31, 6, 4);
insert into ulozeni_zbozi values (59, 31, 6, 6);
insert into ulozeni_zbozi values (62, 31, 6, 7);
insert into ulozeni_zbozi values (15, 31, 6, 8);
insert into ulozeni_zbozi values (55, 31, 6, 9);
insert into ulozeni_zbozi values (29, 31, 6, 11);
insert into ulozeni_zbozi values (19, 31, 6, 12);
insert into ulozeni_zbozi values (25, 31, 6, 13);
insert into ulozeni_zbozi values (51, 31, 6, 14);
insert into ulozeni_zbozi values (27, 31, 6, 15);
insert into ulozeni_zbozi values (69, 31, 6, 16);
insert into ulozeni_zbozi values (51, 31, 6, 17);
insert into ulozeni_zbozi values (22, 31, 6, 19);
insert into ulozeni_zbozi values (25, 6, 1, 1);
insert into ulozeni_zbozi values (40, 5, 1, 2);
insert into ulozeni_zbozi values (19, 5, 1, 3);
insert into ulozeni_zbozi values (26, 4, 1, 4);
insert into ulozeni_zbozi values (24, 3, 1, 5);
insert into ulozeni_zbozi values (28, 6, 1, 6);
insert into ulozeni_zbozi values (54, 5, 1, 7);
insert into ulozeni_zbozi values (39, 5, 1, 8);
insert into ulozeni_zbozi values (38, 4, 1, 9);
insert into ulozeni_zbozi values (58, 7, 1, 10);
insert into ulozeni_zbozi values (41, 6, 1, 11);
insert into ulozeni_zbozi values (49, 7, 1, 12);
insert into ulozeni_zbozi values (68, 6, 1, 13);
insert into ulozeni_zbozi values (17, 6, 1, 14);
insert into ulozeni_zbozi values (33, 4, 1, 15);
insert into ulozeni_zbozi values (37, 7, 1, 16);
insert into ulozeni_zbozi values (27, 3, 1, 17);
insert into ulozeni_zbozi values (65, 7, 1, 18);
insert into ulozeni_zbozi values (57, 7, 1, 19);
insert into ulozeni_zbozi values (40, 3, 1, 20);
insert into ulozeni_zbozi values (66, 9, 2, 1);
insert into ulozeni_zbozi values (47, 9, 2, 2);
insert into ulozeni_zbozi values (38, 9, 2, 3);
insert into ulozeni_zbozi values (51, 13, 2, 4);
insert into ulozeni_zbozi values (11, 10, 2, 5);
insert into ulozeni_zbozi values (41, 11, 2, 6);
insert into ulozeni_zbozi values (14, 11, 2, 7);
insert into ulozeni_zbozi values (24, 11, 2, 8);
insert into ulozeni_zbozi values (68, 10, 2, 10);
insert into ulozeni_zbozi values (49, 9, 2, 11);
insert into ulozeni_zbozi values (59, 11, 2, 12);
insert into ulozeni_zbozi values (27, 11, 2, 13);
insert into ulozeni_zbozi values (31, 13, 2, 14);
insert into ulozeni_zbozi values (67, 12, 2, 16);
insert into ulozeni_zbozi values (29, 11, 2, 17);
insert into ulozeni_zbozi values (51, 13, 2, 18);
insert into ulozeni_zbozi values (15, 11, 2, 19);
insert into ulozeni_zbozi values (44, 9, 2, 20);
insert into ulozeni_zbozi values (42, 19, 3, 1);
insert into ulozeni_zbozi values (56, 17, 3, 2);
insert into ulozeni_zbozi values (28, 16, 3, 3);
insert into ulozeni_zbozi values (65, 15, 3, 4);
insert into ulozeni_zbozi values (49, 18, 3, 5);
insert into ulozeni_zbozi values (62, 16, 3, 6);
insert into ulozeni_zbozi values (57, 18, 3, 7);
insert into ulozeni_zbozi values (18, 18, 3, 8);
insert into ulozeni_zbozi values (23, 17, 3, 9);
insert into ulozeni_zbozi values (24, 19, 3, 10);
insert into ulozeni_zbozi values (58, 19, 3, 11);
insert into ulozeni_zbozi values (66, 18, 3, 12);
insert into ulozeni_zbozi values (20, 17, 3, 13);
insert into ulozeni_zbozi values (64, 16, 3, 14);
insert into ulozeni_zbozi values (13, 16, 3, 15);
insert into ulozeni_zbozi values (26, 16, 3, 16);
insert into ulozeni_zbozi values (51, 19, 3, 17);
insert into ulozeni_zbozi values (69, 15, 3, 18);
insert into ulozeni_zbozi values (18, 18, 3, 19);
insert into ulozeni_zbozi values (26, 18, 3, 20);
insert into ulozeni_zbozi values (65, 23, 4, 2);
insert into ulozeni_zbozi values (63, 23, 4, 3);
insert into ulozeni_zbozi values (37, 22, 4, 4);
insert into ulozeni_zbozi values (40, 23, 4, 5);
insert into ulozeni_zbozi values (60, 24, 4, 7);
insert into ulozeni_zbozi values (21, 24, 4, 9);
insert into ulozeni_zbozi values (33, 24, 4, 10);
insert into ulozeni_zbozi values (31, 25, 4, 11);
insert into ulozeni_zbozi values (50, 23, 4, 12);
insert into ulozeni_zbozi values (65, 21, 4, 13);
insert into ulozeni_zbozi values (38, 22, 4, 14);
insert into ulozeni_zbozi values (67, 21, 4, 15);
insert into ulozeni_zbozi values (63, 25, 4, 16);
insert into ulozeni_zbozi values (15, 22, 4, 17);
insert into ulozeni_zbozi values (56, 23, 4, 18);
insert into ulozeni_zbozi values (67, 25, 4, 19);
insert into ulozeni_zbozi values (22, 24, 4, 20);
insert into ulozeni_zbozi values (59, 29, 5, 1);
insert into ulozeni_zbozi values (62, 28, 5, 2);
insert into ulozeni_zbozi values (18, 29, 5, 4);
insert into ulozeni_zbozi values (64, 29, 5, 5);
insert into ulozeni_zbozi values (40, 30, 5, 7);
insert into ulozeni_zbozi values (38, 28, 5, 8);
insert into ulozeni_zbozi values (60, 28, 5, 9);
insert into ulozeni_zbozi values (21, 30, 5, 10);
insert into ulozeni_zbozi values (64, 30, 5, 11);
insert into ulozeni_zbozi values (18, 29, 5, 12);
insert into ulozeni_zbozi values (17, 28, 5, 13);
insert into ulozeni_zbozi values (41, 29, 5, 14);
insert into ulozeni_zbozi values (17, 28, 5, 15);
insert into ulozeni_zbozi values (47, 29, 5, 16);
insert into ulozeni_zbozi values (30, 28, 5, 17);
insert into ulozeni_zbozi values (13, 27, 5, 18);
insert into ulozeni_zbozi values (26, 28, 5, 19);
insert into ulozeni_zbozi values (45, 30, 5, 20);
insert into ulozeni_zbozi values (50, 35, 6, 1);
insert into ulozeni_zbozi values (39, 32, 6, 2);
insert into ulozeni_zbozi values (68, 35, 6, 4);
insert into ulozeni_zbozi values (36, 33, 6, 6);
insert into ulozeni_zbozi values (49, 32, 6, 7);
insert into ulozeni_zbozi values (70, 33, 6, 8);
insert into ulozeni_zbozi values (64, 35, 6, 9);
insert into ulozeni_zbozi values (58, 33, 6, 11);
insert into ulozeni_zbozi values (35, 32, 6, 12);
insert into ulozeni_zbozi values (16, 35, 6, 13);
insert into ulozeni_zbozi values (41, 32, 6, 14);
insert into ulozeni_zbozi values (27, 33, 6, 15);
insert into ulozeni_zbozi values (54, 32, 6, 16);
insert into ulozeni_zbozi values (14, 35, 6, 17);
insert into ulozeni_zbozi values (48, 34, 6, 19);




insert into prodane_zbozi values(1 , 900, 3, 1, 9);
insert into prodane_zbozi values(1 , 900, 2, 1, 7);
insert into prodane_zbozi values(1 , 900, 2, 1, 17);
insert into prodane_zbozi values(1 , 900, 3, 1, 5);
insert into prodane_zbozi values(2 , 900, 1, 1, 10);
insert into prodane_zbozi values(2 , 900, 1, 1, 17);
insert into prodane_zbozi values(2 , 900, 3, 1, 5);
insert into prodane_zbozi values(2 , 900, 2, 1, 13);
insert into prodane_zbozi values(2 , 900, 2, 1, 19);
insert into prodane_zbozi values(2 , 900, 1, 1, 6);
insert into prodane_zbozi values(2 , 900, 1, 1, 3);
insert into prodane_zbozi values(3 , 900, 2, 1, 2);
insert into prodane_zbozi values(3 , 900, 2, 1, 7);
insert into prodane_zbozi values(3 , 900, 3, 1, 11);
insert into prodane_zbozi values(3 , 900, 1, 1, 9);
insert into prodane_zbozi values(4 , 900, 3, 1, 4);
insert into prodane_zbozi values(4 , 900, 3, 1, 8);
insert into prodane_zbozi values(4 , 900, 3, 1, 12);
insert into prodane_zbozi values(4 , 900, 1, 1, 10);
insert into prodane_zbozi values(4 , 900, 1, 1, 2);
insert into prodane_zbozi values(4 , 900, 1, 1, 1);
insert into prodane_zbozi values(4 , 900, 1, 1, 9);
insert into prodane_zbozi values(4 , 900, 2, 1, 19);
insert into prodane_zbozi values(5 , 900, 1, 1, 15);
insert into prodane_zbozi values(5 , 900, 1, 1, 19);
insert into prodane_zbozi values(5 , 900, 2, 1, 10);
insert into prodane_zbozi values(5 , 900, 2, 1, 3);
insert into prodane_zbozi values(5 , 900, 1, 1, 8);
insert into prodane_zbozi values(6 , 900, 1, 1, 5);
insert into prodane_zbozi values(6 , 900, 3, 1, 1);
insert into prodane_zbozi values(6 , 900, 3, 1, 7);
insert into prodane_zbozi values(6 , 900, 1, 1, 17);
insert into prodane_zbozi values(6 , 900, 1, 1, 2);
insert into prodane_zbozi values(6 , 900, 2, 1, 9);
insert into prodane_zbozi values(7 , 900, 3, 2, 4);
insert into prodane_zbozi values(7 , 900, 1, 2, 8);
insert into prodane_zbozi values(7 , 900, 2, 2, 14);
insert into prodane_zbozi values(7 , 900, 1, 2, 7);
insert into prodane_zbozi values(7 , 900, 3, 2, 16);
insert into prodane_zbozi values(8 , 900, 2, 2, 2);
insert into prodane_zbozi values(8 , 900, 2, 2, 16);
insert into prodane_zbozi values(8 , 900, 2, 2, 19);
insert into prodane_zbozi values(8 , 900, 3, 2, 11);
insert into prodane_zbozi values(9 , 900, 3, 2, 13);
insert into prodane_zbozi values(9 , 900, 3, 2, 17);
insert into prodane_zbozi values(9 , 900, 2, 2, 5);
insert into prodane_zbozi values(10, 2589, 3, 2, 17);
insert into prodane_zbozi values(10, 2589, 3, 2, 8);
insert into prodane_zbozi values(10, 2589, 2, 2, 4);
insert into prodane_zbozi values(10, 2589, 2, 2, 16);
insert into prodane_zbozi values(10, 2589, 3, 2, 1);
insert into prodane_zbozi values(10, 2589, 1, 2, 3);
insert into prodane_zbozi values(10, 2589, 3, 2, 14);
insert into prodane_zbozi values(11, 2589, 1, 2, 8);
insert into prodane_zbozi values(11, 2589, 3, 2, 5);
insert into prodane_zbozi values(11, 2589, 2, 2, 1);
insert into prodane_zbozi values(11, 2589, 2, 2, 6);
insert into prodane_zbozi values(11, 2589, 2, 2, 13);
insert into prodane_zbozi values(11, 2589, 1, 2, 17);
insert into prodane_zbozi values(11, 2589, 2, 2, 12);
insert into prodane_zbozi values(11, 2589, 3, 2, 14);
insert into prodane_zbozi values(12, 2589, 2, 3, 4);
insert into prodane_zbozi values(12, 2589, 1, 3, 2);
insert into prodane_zbozi values(12, 2589, 2, 3, 15);
insert into prodane_zbozi values(12, 2589, 1, 3, 18);
insert into prodane_zbozi values(13, 2589, 1, 3, 18);
insert into prodane_zbozi values(13, 2589, 2, 3, 2);
insert into prodane_zbozi values(13, 2589, 2, 3, 6);
insert into prodane_zbozi values(13, 2589, 1, 3, 16);
insert into prodane_zbozi values(13, 2589, 1, 3, 17);
insert into prodane_zbozi values(14, 2589, 3, 3, 5);
insert into prodane_zbozi values(14, 2589, 3, 3, 2);
insert into prodane_zbozi values(14, 2589, 3, 3, 14);
insert into prodane_zbozi values(15, 2589, 2, 3, 17);
insert into prodane_zbozi values(15, 2589, 3, 3, 14);
insert into prodane_zbozi values(15, 2589, 3, 3, 19);
insert into prodane_zbozi values(15, 2589, 2, 3, 16);
insert into prodane_zbozi values(16, 2589, 1, 4, 18);
insert into prodane_zbozi values(16, 2589, 2, 4, 10);
insert into prodane_zbozi values(16, 2589, 2, 4, 2);
insert into prodane_zbozi values(16, 2589, 2, 4, 17);
insert into prodane_zbozi values(17, 2589, 1, 4, 10);
insert into prodane_zbozi values(17, 2589, 1, 4, 18);
insert into prodane_zbozi values(17, 2589, 2, 4, 16);
insert into prodane_zbozi values(17, 2589, 3, 4, 15);
insert into prodane_zbozi values(17, 2589, 1, 4, 11);
insert into prodane_zbozi values(17, 2589, 1, 4, 13);
insert into prodane_zbozi values(18, 2589, 3, 5, 9);
insert into prodane_zbozi values(18, 2589, 1, 5, 8);
insert into prodane_zbozi values(18, 2589, 1, 5, 12);
insert into prodane_zbozi values(19, 2589, 2, 5, 5);
insert into prodane_zbozi values(19, 2589, 2, 5, 11);
insert into prodane_zbozi values(19, 2589, 1, 5, 12);
insert into prodane_zbozi values(19, 2589, 2, 5, 15);
insert into prodane_zbozi values(19, 2589, 3, 5, 16);
insert into prodane_zbozi values(19, 2589, 2, 5, 14);
insert into prodane_zbozi values(20, 2589, 1, 6, 13);
insert into prodane_zbozi values(20, 2589, 2, 6, 19);
insert into prodane_zbozi values(20, 2589, 1, 6, 12);
insert into prodane_zbozi values(20, 2589, 3, 6, 7);
insert into prodane_zbozi values(20, 2589, 3, 6, 15);
insert into prodane_zbozi values(20, 2589, 3, 6, 6);

insert into zamestnanci values(null, 'admin', 'F489922953333856F597BEC41A942FB6B4ECEF2CACE347D82D283683A5013F23', '0320CF7947EF694AF7EA2AFF919AF0A5', 'Admin', 'Admin', TO_DATE('22-07-2012', 'dd-MM-yyyy'), null, null, '850126/1153');
insert into zamestnanci values(null, 'viktor', '1AAB36BF2EF44EACFA97905F013EBC6750578FFBAC3D005243117FEFF9039BA9', '1234567891234567', 'Viktor', 'Fibonacci', TO_DATE('01-05-2005', 'dd-MM-yyyy'), 1, null, '921201/1237');
insert into zamestnanci values(null, 'donald', 'B6AB830750E3B286E4E2AEE9A606199ED0C424450C3284AEDBBACF4C506044E2', '1234567891234567', 'Donald', 'McGoogle', TO_DATE('22-07-2012', 'dd-MM-yyyy'), 1, null, '620212/2190');

insert into role values(1, 'Pokladnik');
insert into role values(2, 'Manazer');
insert into role values(3, 'Nakladac');
insert into role values(4, 'Admin');

insert into role_zamestnancu values(4, 1);

insert into role_zamestnancu values(1, 2);
insert into role_zamestnancu values(3, 2);

insert into role_zamestnancu values(2, 3);
insert into role_zamestnancu values(3, 3);
insert into role_zamestnancu values(1, 3);

insert into platba values(5000, 1, 1);
insert into platba values(50, 1, 3);

-- hledani role zamestnancu
CREATE OR REPLACE FUNCTION je_role_zamestnancu(p_zamestnanec_id zamestnanci.zamestnanec_id%TYPE, p_role_nazev VARCHAR2)
RETURN INT AS
    v_role_id INT;
    v_count INT;
BEGIN
    SELECT role_id INTO v_role_id FROM ROLE WHERE nazev = p_role_nazev;

    SELECT COUNT(*) INTO v_count FROM ROLE_ZAMESTNANCU WHERE zamestnanec_id = p_zamestnanec_id AND role_id = v_role_id;

    IF v_count > 0 THEN
        RETURN 1;
    ELSE
        RETURN 0;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN 0;
END je_role_zamestnancu;
/

-- nejprodavanější zboží ProductRepository
CREATE OR REPLACE FUNCTION DEJ_NEJPRODAVANEJSI_ZBOZI(p_supermarket_id IN SUPERMARKETY.supermarket_id%TYPE)
RETURN SYS_REFCURSOR IS
    zbozi_rc SYS_REFCURSOR;
BEGIN
    OPEN zbozi_rc FOR 
    SELECT zbozi_id, nazev, pocet_prodeje
    FROM (
        SELECT pz.zbozi_id, z.nazev, COUNT(*) AS pocet_prodeje
        FROM prodane_zbozi pz
        JOIN zbozi z ON z.zbozi_id = pz.zbozi_id
        WHERE pz.supermarket_id = p_supermarket_id
        GROUP BY pz.zbozi_id, z.nazev
        ORDER BY pocet_prodeje DESC)
    WHERE ROWNUM = 1;
    
    RETURN zbozi_rc;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN NULL;
END DEJ_NEJPRODAVANEJSI_ZBOZI;
/
-- možné role zaměstnanců EmployeeRepository
CREATE OR REPLACE FUNCTION DEJ_MOZNE_MANAZERY(p_manazer_id IN zamestnanci.zamestnanec_id%TYPE)
    RETURN SYS_REFCURSOR IS
    v_manazeri_rc SYS_REFCURSOR;
BEGIN
    OPEN v_manazeri_rc FOR
        SELECT z.zamestnanec_id, z.jmeno, z.prijmeni FROM zamestnanci z
        WHERE je_role_zamestnancu(z.zamestnanec_id, 'Manazer') = 1
        CONNECT BY PRIOR z.zamestnanec_id = z.manazer_id
        START WITH z.zamestnanec_id = p_manazer_id;

    RETURN v_manazeri_rc;
END DEJ_MOZNE_MANAZERY;
/






CREATE OR REPLACE PROCEDURE premistit_zbozi(
    old_sklad_id IN NUMBER,
    new_sklad_id IN NUMBER,
    var_zbozi_id IN NUMBER,
    var_kusy IN NUMBER
)
IS
    v_old_kusy NUMBER(6,3);
    v_supermarket_id NUMBER;
    v_transfer_kusy NUMBER;
BEGIN
    SELECT uz.kusy
    INTO v_old_kusy
    FROM ULOZENI_ZBOZI uz
    WHERE uz.misto_ulozeni_id = old_sklad_id
    AND uz.zbozi_id = var_zbozi_id;

    IF v_old_kusy > var_kusy THEN
        v_transfer_kusy := var_kusy;
        UPDATE ULOZENI_ZBOZI uz
        SET uz.kusy = v_old_kusy - var_kusy
        WHERE uz.misto_ulozeni_id = old_sklad_id
        AND uz.zbozi_id = var_zbozi_id;
    ELSE
        v_transfer_kusy := v_old_kusy;
        DELETE FROM ULOZENI_ZBOZI uz
        WHERE misto_ulozeni_id = old_sklad_id
        AND uz.zbozi_id = var_zbozi_id;
    END IF;

    SELECT DISTINCT(uz.supermarket_id)
    INTO v_supermarket_id
    FROM ULOZENI_ZBOZI uz
    WHERE uz.misto_ulozeni_id = new_sklad_id;

    UPDATE ULOZENI_ZBOZI uz
    SET uz.kusy = uz.kusy + v_transfer_kusy
    WHERE uz.misto_ulozeni_id = new_sklad_id
    AND uz.zbozi_id = var_zbozi_id;

    IF SQL%ROWCOUNT = 0 THEN
        INSERT INTO ULOZENI_ZBOZI (kusy, misto_ulozeni_id, supermarket_id, zbozi_id)
        VALUES (v_transfer_kusy, new_sklad_id, v_supermarket_id, var_zbozi_id);
    END IF;
END premistit_zbozi;
/

CREATE OR REPLACE PROCEDURE prijezd_zbozi(
    sklad_id IN NUMBER,
    pr_zbozi_id IN NUMBER,
    pr_supermarket_id IN NUMBER,
    pr_kusy IN NUMBER
)
IS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM ULOZENI_ZBOZI
    WHERE misto_ulozeni_id = sklad_id
    AND zbozi_id = pr_zbozi_id
    AND supermarket_id = pr_supermarket_id;

    IF v_count > 0 THEN
        UPDATE ULOZENI_ZBOZI
        SET kusy = kusy + pr_kusy
        WHERE misto_ulozeni_id = sklad_id
        AND zbozi_id = pr_zbozi_id
        AND supermarket_id = pr_supermarket_id;
    ELSE
        INSERT INTO ULOZENI_ZBOZI (kusy, misto_ulozeni_id, supermarket_id, zbozi_id)
        VALUES (pr_kusy, sklad_id, pr_supermarket_id, pr_zbozi_id);
    END IF;
END prijezd_zbozi;
/

CREATE OR REPLACE PROCEDURE move_and_delete(
  p_misto_ulozeni_id IN MISTA_ULOZENI.misto_ulozeni_id%TYPE,
  p_new_misto_ulozeni_id IN MISTA_ULOZENI.misto_ulozeni_id%TYPE
) IS
  CURSOR c_ulozeni_zbozi IS
    SELECT *
    FROM ULOZENI_ZBOZI
    WHERE misto_ulozeni_id = p_misto_ulozeni_id;
  v_ulozeni_zbozi c_ulozeni_zbozi%ROWTYPE;
BEGIN
  OPEN c_ulozeni_zbozi;
  LOOP
    FETCH c_ulozeni_zbozi INTO v_ulozeni_zbozi;
    EXIT WHEN c_ulozeni_zbozi%NOTFOUND;

    premistit_zbozi(v_ulozeni_zbozi.misto_ulozeni_id, p_new_misto_ulozeni_id, v_ulozeni_zbozi.zbozi_id, v_ulozeni_zbozi.kusy);
  END LOOP;
  CLOSE c_ulozeni_zbozi;

  DELETE FROM MISTA_ULOZENI
  WHERE misto_ulozeni_id = p_misto_ulozeni_id;
END move_and_delete;
/
-- edit zamestnance EmployeesRepository
CREATE OR REPLACE PROCEDURE EDIT_ZAMESTNANCE(
    p_zamestnanec_id ZAMESTNANCI.zamestnanec_id%TYPE,
    p_login ZAMESTNANCI.login%TYPE,
    p_heslo_hash ZAMESTNANCI.heslo_hash%TYPE,
    p_heslo_salt ZAMESTNANCI.heslo_salt%TYPE,
    p_jmeno ZAMESTNANCI.jmeno%TYPE,
    p_prijmeni ZAMESTNANCI.prijmeni%TYPE,
    p_datum_nastupu ZAMESTNANCI.datum_nastupu%TYPE,
    p_supermarket_id ZAMESTNANCI.supermarket_id%TYPE,
    p_manazer_id ZAMESTNANCI.manazer_id%TYPE,
    p_rodne_cislo ZAMESTNANCI.rodne_cislo%TYPE,
    p_je_pokladnik NUMBER,
    p_je_nakladac NUMBER,
    p_je_manazer NUMBER,
    p_je_admin NUMBER
) AS
    v_role_pokladnik_id ROLE.role_id%TYPE;
    v_role_nakladac_id ROLE.role_id%TYPE;
    v_role_manazer_id ROLE.role_id%TYPE;
    v_role_admin_id ROLE.role_id%TYPE;
BEGIN
    UPDATE ZAMESTNANCI
    SET
        login = p_login,
        heslo_hash = p_heslo_hash,
        heslo_salt = p_heslo_salt,
        jmeno = p_jmeno,
        prijmeni = p_prijmeni,
        datum_nastupu = p_datum_nastupu,
        supermarket_id = p_supermarket_id,
        manazer_id = p_manazer_id,
        rodne_cislo = p_rodne_cislo
    WHERE zamestnanec_id = p_zamestnanec_id;

    -- pokracujeme pokud je takovy zamestnanec
    IF SQL%ROWCOUNT != 0 THEN
        DELETE ROLE_ZAMESTNANCU WHERE zamestnanec_id = p_zamestnanec_id;

        IF p_je_pokladnik = 1 THEN
            SELECT role_id INTO v_role_pokladnik_id FROM ROLE WHERE nazev = 'Pokladnik';
            INSERT INTO ROLE_ZAMESTNANCU VALUES (v_role_pokladnik_id, p_zamestnanec_id);
        END IF;

        IF p_je_nakladac = 1 THEN
            SELECT role_id INTO v_role_nakladac_id FROM ROLE WHERE nazev = 'Nakladac';
            INSERT INTO ROLE_ZAMESTNANCU VALUES (v_role_nakladac_id, p_zamestnanec_id);
        END IF;

        IF p_je_manazer = 1 THEN
            SELECT role_id INTO v_role_manazer_id FROM ROLE WHERE nazev = 'Manazer';
            INSERT INTO ROLE_ZAMESTNANCU VALUES (v_role_manazer_id, p_zamestnanec_id);
        END IF;

        IF p_je_admin = 1 THEN
            SELECT role_id INTO v_role_admin_id FROM ROLE WHERE nazev = 'Admin';
            INSERT INTO ROLE_ZAMESTNANCU VALUES (v_role_admin_id, p_zamestnanec_id);
        END IF;
    END IF;
END EDIT_ZAMESTNANCE;
/





CREATE OR REPLACE TRIGGER DODAVATELE_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON DODAVATELE
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DODAVATELE','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DODAVATELE','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DODAVATELE','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER DRUHY_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON DRUHY_ZBOZI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DRUHY_ZBOZI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DRUHY_ZBOZI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('DRUHY_ZBOZI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER MERNE_JEDNOTKY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON MERNE_JEDNOTKY
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MERNE_JEDNOTKY','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MERNE_JEDNOTKY','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MERNE_JEDNOTKY','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER MISTA_ULOZENI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON MISTA_ULOZENI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MISTA_ULOZENI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MISTA_ULOZENI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('MISTA_ULOZENI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PLATBA_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PLATBA
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PLATBA','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PLATBA','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PLATBA','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER POKLADNY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON POKLADNY
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('POKLADNY','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('POKLADNY','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('POKLADNY','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PRODANE_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PRODANE_ZBOZI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODANE_ZBOZI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODANE_ZBOZI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODANE_ZBOZI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PRODAVANE_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PRODAVANE_ZBOZI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODAVANE_ZBOZI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODAVANE_ZBOZI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODAVANE_ZBOZI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER PRODEJE_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON PRODEJE
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODEJE','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODEJE','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('PRODEJE','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER REGIONY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON REGIONY
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('REGIONY','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('REGIONY','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('REGIONY','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ROLE_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ROLE
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ROLE_ZAMESTNANCU_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ROLE_ZAMESTNANCU
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE_ZAMESTNANCU','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE_ZAMESTNANCU','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ROLE_ZAMESTNANCU','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER SOUBORY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON SOUBORY
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SOUBORY','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SOUBORY','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SOUBORY','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER SUPERMARKETY_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON SUPERMARKETY
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SUPERMARKETY','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SUPERMARKETY','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('SUPERMARKETY','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER TYPY_PLACENI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON TYPY_PLACENI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('TYPY_PLACENI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('TYPY_PLACENI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('TYPY_PLACENI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ULOZENI_ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ULOZENI_ZBOZI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ULOZENI_ZBOZI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ULOZENI_ZBOZI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ULOZENI_ZBOZI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ZAMESTNANCI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ZAMESTNANCI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZAMESTNANCI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZAMESTNANCI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZAMESTNANCI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
CREATE OR REPLACE TRIGGER ZBOZI_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON ZBOZI
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZBOZI','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZBOZI','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('ZBOZI','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/


CREATE OR REPLACE TRIGGER trg_before_insert_soubory
BEFORE INSERT ON SOUBORY
FOR EACH ROW
BEGIN
  :NEW.datum_nahrani := SYSTIMESTAMP;
  :NEW.datum_modifikace := SYSTIMESTAMP;
END;
/

CREATE OR REPLACE TRIGGER trg_set_null_supermarket_id
BEFORE INSERT OR UPDATE ON ZAMESTNANCI
FOR EACH ROW
DECLARE
    v_role_id NUMBER;
BEGIN
    BEGIN
        SELECT role_id INTO v_role_id FROM ROLE_ZAMESTNANCU WHERE zamestnanec_id = :NEW.zamestnanec_id AND role_id = 4;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_role_id := NULL;
    END;

    IF v_role_id IS NOT NULL THEN
        :NEW.supermarket_id := NULL;
    END IF;
END;
/



DROP VIEW SalesView;
DROP VIEW AdminProductsView;
DROP VIEW ManagerEmployeeDetail;

-- #1 SaleRepository
CREATE OR REPLACE VIEW SalesView AS
WITH AggregatedZbozi AS (
    SELECT
        prodej_id,
        LISTAGG(DISTINCT z.nazev || ' ' || pz.kusy || mj.zkratka || ' ' || pz.celkova_cena || 'Kč', ', ') WITHIN GROUP (ORDER BY z.nazev) AS zbozi
    FROM
        PRODANE_ZBOZI pz
    JOIN
        ZBOZI z ON z.zbozi_id = pz.zbozi_id
    JOIN
        MERNE_JEDNOTKY mj ON z.merna_jednotka_id = mj.merna_jednotka_id
    GROUP BY
        prodej_id
)

SELECT
    p.prodej_id,
    p.datum,
    pk.pokladna_id,
    pk.nazev as pokladna_nazev,
    SUM(pl.castka) as cena,
    LISTAGG(DISTINCT tp.nazev, ', ') WITHIN GROUP (ORDER BY tp.nazev) AS typ_placeni_nazev,
    az.zbozi,
    pk.supermarket_id
FROM
    PRODEJE p
JOIN
    POKLADNY pk ON pk.pokladna_id = p.pokladna_id
LEFT JOIN
    PLATBA pl ON pl.prodej_id = p.prodej_id
LEFT JOIN
    TYPY_PLACENI tp ON tp.typ_placeni_id = pl.typ_placeni_id
JOIN
    AggregatedZbozi az ON az.prodej_id = p.prodej_id
GROUP BY
    p.prodej_id,
    p.datum,
    pk.pokladna_id,
    pk.nazev,
    az.zbozi,
    pk.supermarket_id;
/

--#2 ProductRepository
CREATE OR REPLACE VIEW AdminProductsView AS
SELECT
                        z.zbozi_id as zbozi_id,
                        z.nazev as nazev,
                        z.navahu as navahu,
                        z.cena as cena,
                        z.carovykod as carovykod,
                        d.dodavatel_id as dodavatel_id,
                        d.nazev as dodavatel_nazev,
                        z.popis as popis,
                        z.merna_jednotka_id,
                        mj.nazev as merna_jednotka_nazev,
                        dz.druh_zbozi_id as druh_id,
                        dz.nazev as druh_nazev
                    FROM
                        ZBOZI z
                    JOIN
                        DODAVATELE d on d.dodavatel_id = z.dodavatel_id
                    JOIN
                        MERNE_JEDNOTKY mj on mj.merna_jednotka_id = z.merna_jednotka_id
                    JOIN
                        DRUHY_ZBOZI dz on dz.druh_zbozi_id = z.druh_zbozi_id;
/
--#3  EmployeeRepository                      
CREATE OR REPLACE VIEW ManagerEmployeeDetail AS
SELECT
    z.zamestnanec_id,
    z.jmeno,
    z.prijmeni,
    z.datum_nastupu,
    z.login,
    z.manazer_id,
    z.supermarket_id,
    MAX(CASE WHEN r.role_id = 1 THEN 1 ELSE 0 END) AS isPokladnik,
    MAX(CASE WHEN r.role_id = 2 THEN 1 ELSE 0 END) AS isManazer,
    MAX(CASE WHEN r.role_id = 3 THEN 1 ELSE 0 END) AS isNakladac
FROM
    ZAMESTNANCI z
LEFT JOIN
    ROLE_ZAMESTNANCU rz ON z.zamestnanec_id = rz.zamestnanec_id
LEFT JOIN
    ROLE r ON rz.role_id = r.role_id
GROUP BY
    z.zamestnanec_id, z.jmeno, z.prijmeni, z.datum_nastupu, z.login, z.manazer_id, z.supermarket_id;
/