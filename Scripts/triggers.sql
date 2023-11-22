CREATE OR REPLACE TRIGGER trg_before_insert_soubory
BEFORE INSERT ON SOUBORY
FOR EACH ROW
BEGIN
  :NEW.datum_nahrani := SYSTIMESTAMP;
END;
/

CREATE OR REPLACE TRIGGER trg_set_null_supermarket_id
BEFORE INSERT OR UPDATE ON ZAMESTNANCI
FOR EACH ROW
DECLARE
    v_role_id NUMBER;
BEGIN
    BEGIN
        SELECT role_id INTO v_role_id FROM ROLE_ZAMESTNANCU WHERE zamestnanec_id = :NEW.zamestnanec_id AND role_id = 1;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            v_role_id := NULL;
    END;

    IF v_role_id IS NOT NULL THEN
        :NEW.supermarket_id := NULL;
    END IF;
END;
/