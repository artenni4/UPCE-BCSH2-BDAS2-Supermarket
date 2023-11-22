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
