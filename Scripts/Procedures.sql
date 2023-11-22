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
END;
/
