# Использование FTS в PostgreSQL
#### 1. Тип tsvector
Выполните запрос
```sql
SELECT to_tsvector('The quick brown fox jumped over the lazy dog.');
```
В ответ будет возвращён список [лексем](https://en.wikipedia.org/wiki/Lexeme)
```sql
                to_tsvector
'brown':3 'dog':9 'fox':4 'jump':5 'lazi':8 'quick':2
```

> Задание 1
Потому что данное слово входит в список стоп-слов, которые не учитываются при полнотекстовом поиске, т.к встречаются очень часто
#### 2. Тип tsquery
Выполните по очереди запросы
```sql
--№1
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('fox');
--№2
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('foxes');
--№3 
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('foxhound');
```

> Задание 2
1. Что означают символы `@@`
*оператор, который показывает соответствие между tsvector и tsquery
2. Почему второй запрос возвращает `true`, а третий не возвращает
*потому что лексема от "foxes" это "fox", а лексема от "foxhound" это "foxhound"
3. Выполните запрос
```sql
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.')
    @@ to_tsquery('Russian','булка');
```
Почему слово булка не находится?
4. Используйте функцию `select ts_lexize('russian_stem', 'булок');` для того чтобы понять почему.
*Лексема от "булок" - "булок", а лексема от "булка" - "булк".
5. Замените в предложении слово `булок`, на слово `пирожков`
Выполните запросы
```sql
--№1
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')
    @@ to_tsquery('Russian','пирожки');
--№2
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')
    @@ to_tsquery('Russian','пирожок');
```
Почему первый запрос возвращает `true`, а второй не возвращает?
*"пирожки" и "пирожков" имеют лексему "пирожк". Слово "пирожок" имеет лексему "пирожок".
#### 3. Операторы
Выполните запрос
```sql
--И
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('fox & dog');

--ИЛИ
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('fox | rat');

--отрицание
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('!clown');

--группировка
SELECT to_tsvector('The quick brown fox jumped over the lazy dog')  
    @@ to_tsquery('fox & (dog | rat) & !mice');
```
> Задание 3
1. Приведите аналогичные запросы для любого предложения на русском
```sql
--И
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')  
  @@ to_tsquery('Russian', 'пирожки & чай');

--ИЛИ
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')  
  @@ to_tsquery('Russian', 'пирожок | чай');

--отрицание
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')  
  @@ to_tsquery('Russian', '!Река');

--группировка
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.')  
  @@ to_tsquery('Russian', 'пирожки & (чай | река) & !лес');
```
2. Почему для английского языка не нужно указывать язык в первом аргументе и какой анализатор используется если никакой не указан?
*Для английского необязательно указывать язык, т.к. при отсутствии этого параметра, используется default configuration в котором используется английский

#### 4. Поиск фраз
Изучите документацию по [операторам](https://www.postgresql.org/docs/current/functions-textsearch.html) FTS
Выполните запрос
```sql

SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.')
    @@ to_tsquery('Russian','мягких<2>булок');
```
> Задание 4
1. Что означает число 2 в операторе `<->`
*Допускает, что между искомыми лексемами находится ещё ровно**одна**
2. Модифицируйте запрос так, чтобы можно было найти фразу `съешь ещё`
*
```sql

SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.')
    @@ to_tsquery('Russian','съешь<->ещё');
```
3. Для чего нужно использовать функцию `phraseto_tsquery`

#### 5. Утилиты
1. Приведите примеры использования функций `ts_debug` и  `ts_headline`
```sql
--ts_debug
SELECT * FROM ts_debug('english',
  'a fat  cat sat on a mat');
```
```sql
   alias  |   description  | token|  dictionaries |  dictionary |lexemes
----------+----------------+------+---------------+-------------+-------
 asciiword| Word, all ASCII| a    | {english_stem}| english_stem| {}
 blank    | Space symbols  |      | {}            |             | 
 asciiword| Word, all ASCII| fat  | {english_stem}| english_stem| {fat}
 blank    | Space symbols  |      | {}            |             | 
 asciiword| Word, all ASCII| cat  | {english_stem}| english_stem| {cat}
 blank    | Space symbols  |      | {}            |             | 
 asciiword| Word, all ASCII| sat  | {english_stem}| english_stem| {sat}
 blank    | Space symbols  |      | {}            |             | 
 asciiword| Word, all ASCII| on   | {english_stem}| english_stem| {}
 blank    | Space symbols  |      | {}            |             | 
 asciiword| Word, all ASCII| a    | {english_stem}| english_stem| {}
 blank    | Space symbols  |      | {}            |             | 
 asciiword| Word, all ASCII| mat  | {english_stem}| english_stem| {mat}
 ```

```sql
--ts_headline
SELECT ts_headline('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.',, to_tsquery('Russian','Съешь<->булок')),
  'StartSel = <, StopSel = >');
```
**Съешь** ещё этих мягких французских **булок**, да выпей чаю.