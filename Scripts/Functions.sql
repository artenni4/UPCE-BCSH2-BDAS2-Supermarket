-- hledani role zamestnancu
CREATE OR REPLACE FUNCTION je_role_zamestnancu(p_zamestnanec_id INT, p_role_nazev VARCHAR2)
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
END;