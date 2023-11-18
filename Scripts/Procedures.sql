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


