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