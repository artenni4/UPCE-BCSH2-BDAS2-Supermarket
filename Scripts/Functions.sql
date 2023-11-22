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