%macro cout 2
    mov rax, 1
    mov rdi, 1
    mov rsi, %1
    mov rdx, %2
    syscall
%endmacro

section .text
global _start

_start:
    lea rsi, [x] ; начало массва x
    mov rcx, count ; кол-во итерраций
    mov r12, 0 ; сюда будем добавлять, потом вычитать

first_loop:
    cmp rcx, 0
    jz next

    mov eax, [rsi]
    movsx rbx, eax ; расширяем EAX со знаком в 64-битный RBX
    add r12, rbx ; прибавляем значение
    
    add rsi, 4 ; сдвигаем указатель на 4 байта т.к dd

    dec rcx
    jnz first_loop

next:
    lea rsi, [y]
    mov rcx, count

second_loop:
    cmp rcx, 0
    jz division
    
    mov eax, [rsi]
    movsx rbx, eax
    sub r12, rbx ; вычитаем значение

    add rsi, 4
    
    dec rcx
    jnz second_loop

division:
    mov rax, r12
    mov rbx, 1000 ; для точности 3 знаков
    imul rbx

    mov rbx, count
    cqo
    idiv rbx

;вывод
print:
    mov rcx, resultBuff
    
    cmp rax, 0 ; проверка на -
    jns convert

    mov byte [rcx], '-'
    inc rcx
    neg rax

convert:
    mov rbx, 10
    mov r8, 0 ; счетчик цифр в числе

convert_loop:
    xor rdx, rdx
    div rbx
    add rdx, '0'
    push rdx
    inc r8
    cmp rax, 0
    jnz convert_loop

    mov r9, r8
    sub r9, accur ; кол-во в целой части

    cmp r9, 0 ; елси только дробная часть
    jle print_zero

print_int:
    cmp r9, 0
    jz print_point ; вывели всю целую часть, ставим точку

    pop rax
    mov [rcx], al
    inc rcx
    dec r9
    jmp print_int

print_zero:
    mov byte [rcx], '0'
    inc rcx

print_point:
    mov byte [rcx], '.'
    inc rcx

    mov r9, r8
    sub r9, accur
    cmp r9, 0
    jge pop_fraction_part

    mov r10, accur
    sub r10, r8
    print_zeros:
        cmp r10, 0
        jle pop_fraction_part
        mov byte [rcx], '0'
        inc rcx
        dec r10
        jmp print_zeros

pop_fraction_part:
    pop rax
    mov [rcx], al
    inc rcx
    dec r8
    jnz pop_fraction_part

do_print:
    mov rdx, rcx
    sub rdx, resultBuff
    cout resultBuff, rdx
    cout newline, 1

exit:
    xor rdi, rdi
    mov rax, 60
    syscall

section .data
    x dd 5, 3, 2, 6, 1, 7, 4 

    y dd 0, 10, 1, 9, 2, 8, 10
    y_end:
    
    count equ (y_end - y) / 4 ; берем любой массив т.к равны
    accur equ 3 

    newline    db 10

section .bss
    is_negative resb 1
    resultBuff resb 32
