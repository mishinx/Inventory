-- Створення таблиці адміністраторів
CREATE TABLE administrators (
    admin_id serial PRIMARY KEY,
    company_name text,
    email_address text,
    admin_password text,
    full_name text,
    phone_number text
);

-- Створення таблиці складів
CREATE TABLE warehouses (
    warehouse_id serial PRIMARY KEY,
    addres text,
    admins_id integer REFERENCES administrators(admin_id)
);

-- Створення таблиці товарів
CREATE TABLE goods (
    goods_id serial PRIMARY KEY,
    full_name text,
    category text,
    subcategory text,
    short_description text,
    quantity integer,
    price numeric(10, 2),
    warehouses_id integer REFERENCES warehouses(warehouse_id),
    Photo bytea
);

-- Створення таблиці операторів складу
CREATE TABLE operators (
    operator_id serial PRIMARY KEY ,
    email_address text,
    admin_password text,
    full_name text,
    phone_number text,
    warehouses_id integer REFERENCES warehouses(warehouse_id),
    admins_id integer REFERENCES administrators(admin_id)
);
