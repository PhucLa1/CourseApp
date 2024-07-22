"use client"
import React from 'react'
import MenuNavbar from './_components/MenuNavbar'


export default function layout({ children }: { children: React.ReactNode }) {
  return (
    <div style={{ margin: 0, fontSize: '0.875rem', fontWeight: 400, lineHeight: 1.5, color: '#7c8fac', backgroundColor: '#202936', WebkitTextSizeAdjust: '100%', WebkitTapHighlightColor: 'transparent' }}>
      <div style={{ minHeight: '100vh' }} >
        <MenuNavbar />
        <div style={{ marginLeft: '270px',boxSizing:'border-box', padding:'3rem' }}>
          {children}
        </div>

      </div>
    </div>
  )
}
