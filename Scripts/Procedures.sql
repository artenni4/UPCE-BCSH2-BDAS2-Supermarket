CREATE OR REPLACE PROCEDURE premistit_zbozi(
    old_sklad_id IN NUMBER,
    new_sklad_id IN NUMBER,
    var_zbozi_id IN NUMBER,
    var_kusy IN NUMBER
)
IS
    v_old_kusy NUMBER(6,3);
    v_supermarket_id NUMBER;
BEGIN
    SELECT uz.kusy
    INTO v_old_kusy
    FROM ULOZENI_ZBOZI uz
    WHERE uz.misto_ulozeni_id = old_sklad_id
    AND uz.zbozi_id = var_zbozi_id;

    IF v_old_kusy > var_kusy THEN
        UPDATE ULOZENI_ZBOZI uz
        SET uz.kusy = v_old_kusy - var_kusy
        WHERE uz.misto_ulozeni_id = old_sklad_id
        AND uz.zbozi_id = var_zbozi_id;

        SELECT DISTINCT(uz.supermarket_id)
        INTO v_supermarket_id
        FROM ULOZENI_ZBOZI uz
        WHERE uz.misto_ulozeni_id = new_sklad_id;

        UPDATE ULOZENI_ZBOZI uz
        SET uz.kusy = uz.kusy + var_kusy
        WHERE uz.misto_ulozeni_id = new_sklad_id
        AND uz.zbozi_id = var_zbozi_id;

        IF SQL%ROWCOUNT = 0 THEN
            INSERT INTO ULOZENI_ZBOZI (kusy, misto_ulozeni_id, supermarket_id, zbozi_id)
            VALUES (var_kusy, new_sklad_id, v_supermarket_id, var_zbozi_id);
        END IF;
    ELSE
        DELETE FROM ULOZENI_ZBOZI uz
        WHERE misto_ulozeni_id = old_sklad_id
        AND uz.zbozi_id = var_zbozi_id;
    END IF;
END premistit_zbozi;


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