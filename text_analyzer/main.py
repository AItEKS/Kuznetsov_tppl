f = open("/home/egor/Учёба/Теория и практика ЯП/Lesson 4 - 5/file.txt").readlines()

print(f"Кол-во строк: {len(f)}")
print(f"Кол-во символов: {sum([len(i) for i in f])}")
print(f"Кол-во пустых строк: {sum([1 for i in f if i == '\n'])}")
print("Словарь: ", {i: sum(line.count(i) for line in f) for i in set(''.join(f))})
