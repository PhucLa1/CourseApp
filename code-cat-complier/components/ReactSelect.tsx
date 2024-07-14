import React from 'react';
import Select, { MultiValue } from 'react-select';
import makeAnimated from 'react-select/animated';
import { number } from 'zod';

// Định nghĩa dữ liệu tùy chọn


const animatedComponents = makeAnimated();

// Định nghĩa phong cách tùy chỉnh cho react-select
const customStyles = {
    control: (provided: any) => ({
        ...provided,
        backgroundColor: '222.2 84% 4.9%',
        color: 'white',
        fontSize: '14px', // Cỡ chữ lớn hơn
    }),
    menu: (provided: any) => ({
        ...provided,
        backgroundColor: '222.2 84% 4.9%',
        color: 'white',
        fontSize: '14px', // Cỡ chữ lớn hơn
    }),
    option: (provided: any, { isFocused, isSelected }: any) => ({
        ...provided,
        backgroundColor: isFocused ? 'var(--bg-card-focus)' : isSelected ? 'var(--bg-card-selected)' : 'var(--bg-card)',
        color: 'white',
        fontSize: '14px', // Cỡ chữ lớn hơn
    }),
    multiValue: (provided: any) => ({
        ...provided,
        backgroundColor: 'var(--bg-card)',
        color: 'white',
        fontSize: '14px', // Cỡ chữ lớn hơn
    }),
    multiValueLabel: (provided: any) => ({
        ...provided,
        color: 'white',
        fontSize: '14px', // Cỡ chữ lớn hơn
    }),
    multiValueRemove: (provided: any) => ({
        ...provided,
        color: 'white',
        fontSize: '14px', // Cỡ chữ lớn hơn
        ':hover': {
            backgroundColor: 'var(--bg-card-hover)',
            color: 'white',
        },
    }),
};
type Value = {
    label: any,
    value: any
}
export default function ReactSelect({ value, onHandleTagIds }: { value: Value[], onHandleTagIds: (data: number[]) => void }) {
    const HandleTagIds = (newValue: Value[]) => {
        const dataReturn: number[] = newValue.map((item) => item.value);
        onHandleTagIds(dataReturn) // Kiểm tra xem giá trị đã chuyển đổi thành số chưa
    };
    return (
        <Select
            onChange={(e) => HandleTagIds(e)}
            closeMenuOnSelect={false}
            components={animatedComponents}
            isMulti
            options={value}
            styles={customStyles}
            classNamePrefix="bg-card" // Áp dụng lớp CSS cho các thành phần con
        />
    );
}
