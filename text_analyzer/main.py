f_name = input("Путь до файа: ")
f = open(f_name).readlines()

wht_want = [int(i) for i in input("Что вы ходите узнать: \n1. Кол-во строк\n2. Кол-во символов\n3. Кол-во пустых строк\n4. Словарь\nВводить через пробел: ").split()]
ans = [len(f), sum([len(i) for i in f]), sum([1 for i in f if i == '\n']), {i: sum(line.count(i) for line in f) for i in set(''.join(f))}]

for i in wht_want:
    print(ans[i-1])
