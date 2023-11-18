tables = [
    'dodavatele',
    'druhy_zbozi',
    #'logy',
    'merne_jednotky',
    'mista_ulozeni',
    'platba',
    'pokladny',
    'prodane_zbozi',
    'prodavane_zbozi',
    'prodeje',
    'regiony',
    'role',
    'role_zamestnancu',
    'soubory',
    'supermarkety',
    'typy_placeni',
    'ulozeni_zbozi',
    'zamestnanci',
    'zbozi',
]


template = '''CREATE OR REPLACE TRIGGER {0}_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON {0}
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('{0}','INSERT',SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('{0}','UPDATE',SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('{0}','DELETE',SYSTIMESTAMP,USER);
    END IF;
END;
/
'''

with open('log_triggers.sql', '+w') as f:
    for table in tables:
        formatted = template.format(table.upper())
        f.write(formatted)