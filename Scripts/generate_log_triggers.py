'''
SELECT
    '"' || lower(utc.table_name) || 
    '": [' || LISTAGG('"' || lower(utc.column_name) || '"', ', ') WITHIN GROUP (ORDER BY utc.column_id) || '],' AS column_names
FROM 
    user_tab_columns utc
WHERE
    utc.data_type NOT IN ('RAW', 'BLOB', 'CLOB', 'NCLOB', 'BFILE') AND
    lower(utc.table_name) in (
    'dodavatele',
    'druhy_zbozi',
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
    'soubory')
GROUP BY 
    utc.table_name;
'''

tables = {
    "dodavatele": ["dodavatel_id", "nazev"],
    "druhy_zbozi": ["druh_zbozi_id", "nazev", "popis"],
    "merne_jednotky": ["merna_jednotka_id", "nazev", "zkratka"],
    "mista_ulozeni": ["misto_ulozeni_id", "kod", "poloha", "supermarket_id", "misto_ulozeni_typ"],
    "platba": ["castka", "prodej_id", "typ_placeni_id"],
    "pokladny": ["pokladna_id", "supermarket_id", "nazev", "kod", "poznamky"],
    "prodane_zbozi": ["prodej_id", "celkova_cena", "kusy", "supermarket_id", "zbozi_id"],
    "prodavane_zbozi": ["zbozi_id", "supermarket_id", "aktivni"],
    "prodeje": ["prodej_id", "datum", "pokladna_id"],
    "regiony": ["region_id", "nazev"],
    "role": ["role_id", "nazev"],
    "role_zamestnancu": ["role_id", "zamestnanec_id"],
    "soubory": ["soubor_id", "nazev_souboru", "pripona", "datum_nahrani", "supermarket_id", "zamestnanec_id", "datum_modifikace"],
    "supermarkety": ["supermarket_id", "adresa", "region_id"],
    "typy_placeni": ["typ_placeni_id", "nazev"],
    "ulozeni_zbozi": ["kusy", "misto_ulozeni_id", "supermarket_id", "zbozi_id"],
    "zamestnanci": ["zamestnanec_id", "login", "jmeno", "prijmeni", "datum_nastupu", "supermarket_id", "manazer_id", "rodne_cislo"],
    "zbozi": ["zbozi_id", "druh_zbozi_id", "merna_jednotka_id", "navahu", "nazev", "cena", "carovykod", "popis", "dodavatel_id"],
}

template = '''CREATE OR REPLACE TRIGGER {0}_LOG_DML BEFORE INSERT OR UPDATE OR DELETE ON {0}
FOR EACH ROW
BEGIN
    IF INSERTING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('{0}','INSERT' || {1},SYSTIMESTAMP,USER);
    END IF;
    IF UPDATING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('{0}','UPDATE' || {2},SYSTIMESTAMP,USER);
    END IF;
    IF DELETING THEN
        INSERT INTO LOGY(tabulka,operace,cas,uzivatel) VALUES('{0}','DELETE' || {3},SYSTIMESTAMP,USER);
    END IF;
END;
/
'''

def insert_logs(table_columns):
    return ' || '.join(map(lambda c: f"'; {c}:' || :NEW.{c}", table_columns))

def update_logs(table_columns):
    return ' || '.join(map(lambda c: f"'; {c}:' || :OLD.{c} || ' -> ' || :NEW.{c}", table_columns))

def delete_logs(table_columns):
    return ' || '.join(map(lambda c: f"'; {c}:' || :OLD.{c}", table_columns))

with open('log_triggers.sql', '+w') as f:
    for table_name, table_columns in tables.items():
        formatted = template.format(table_name.upper(), insert_logs(table_columns), update_logs(table_columns), delete_logs(table_columns))
        f.write(formatted)