
import Link from 'next/link'
import React from 'react'
import './MenuNavbar.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { FactoryIcon } from 'lucide-react'
import { faBook, faFaceAngry, faTag, faTurnDown } from '@fortawesome/free-solid-svg-icons'
import { title } from 'process'
const navBarMenu = [
  {
    title: "Quản lí bài tập",
    content: [
      { title: "Nhãn dán", link: "/admin/tag-exercise" },
      { title: "Bài tập", link: "/admin/exercises" },
    ]
  },
  {
    title: "Quản lí khóa học",
    content: [
      { title: "Loại khóa học", link: "/admin/course-type" },
      { title: "Thêm mới khóa học", link: "/admin/course/add" },
    ]
  }
]
export default function MenuNavbar() {
  return (
    <aside style={{ width: '270px', borderRight: '1px solid #333f55', flexShrink: 0, background: '#202936', zIndex: 99, transition: '.2s ease-in', position: 'fixed', left: 0, right: 0, height: '100%', display: 'block' }}>
      <div>
        <div className='min-h-[70px] flex items-center justify-between' style={{ padding: '0 24px' }}>
          <Link className='text-nowrap' style={{ textDecoration: 'none', color: 'rgb(124, 143, 172)' }} href='#'>
            <img src="https://bootstrapdemos.adminmart.com/modernize/dist/assets/images/logos/dark-logo.svg" style={{ verticalAlign: 'middle', display: 'none' }} alt="" />
            <img src="https://bootstrapdemos.adminmart.com/modernize/dist/assets/images/logos/light-logo.svg" alt="" />
          </Link>
        </div>
        <nav className='' style={{ overflowY: 'auto', padding: '0 24px', height: 'calc(100vh - 175px)', borderRadius: '7px', position: 'relative', display: 'flex', flexDirection: 'column', flexWrap: 'wrap', justifyContent: 'flex-start', alignContent: 'flex-start', }}>
          <div style={{ margin: '0 -24px', overflow: 'hidden', width: 'inherit', height: 'inherit', maxWidth: 'inherit', maxHeight: 'inherit', }} className=''>
            <div className='simplebar-height-auto-observer-wrapper'>
              <div className='simplebar-height-auto-observer'>
              </div>
            </div>
            <div className='simplebar-mask'>
              <div className='simplebar-offset'>
                <div className='simplebar-content-wrapper'>
                  <div className='simplebar-content'>
                    <ul id='sibarnav'>
                      {navBarMenu.map((item, index) => {
                        return <div key={index}>
                          <li className='nav-small-cap'>
                            <i className='ti'></i>
                            <span>{item.title}</span>
                          </li>
                          {item.content.map((subItem, subIndex) => {
                            return <li key={subIndex} className='side-bar-item hover:text-slate-50'>
                              <Link className='sidebar-link' href={subItem.link}>
                                <span>
                                  <FontAwesomeIcon icon={faTag} />
                                </span>
                                <span>{subItem.title}</span>
                              </Link>
                            </li>
                          })}
                        </div>
                      })}
                    </ul>
                  </div>
                </div>
              </div>
            </div>
            <div className='simplebar-placeholder'></div>
          </div>
          <div style={{ zIndex: 1, position: 'absolute', right: 0, bottom: 0, pointerEvents: 'none', overflow: 'hidden', left: 0, height: '11px', }} className=''>
            <div style={{ right: 'auto', left: 0, top: 0, bottom: 0, minHeight: 0, minWidth: '10px', width: '0px', display: 'none', }}></div>
          </div>
          <div style={{ top: 0, width: '11px', zIndex: 1, position: 'absolute', right: 0, bottom: 0, pointerEvents: 'none', overflow: 'hidden' }} className=''>
            <div className='simplebar-scrollbar' style={{ height: '77px', transform: 'translate3d(0px, 0px, 0px)', display: 'block' }}></div>
          </div>
        </nav>
        <div style={{ position: 'fixed', borderRadius: '7px', backgroundColor: 'rgba(73, 190, 255, 0.1)' }} className='p-3 mb-2 mt-3 mx-4 '>
          <div className='gap-3 hstack'>
            <div className=''>
              <img src="https://bootstrapdemos.adminmart.com/modernize/dist/assets/images/profile/user-1.jpg" className='rounded-md' width="40" height="40" alt="modernize-img"
              />
            </div>
            <div>
              <h6 className='mb-0 text-slate-50' style={{ fontSize: '1rem', fontWeight: '600' }}>Mathew</h6>
              <span className='' style={{ fontSize: '0.75rem' }}>Designer</span>
            </div>
            <button style={{ marginLeft: '4.5rem', backgroundClip: 'transparent', border: 0 }}>
              <FontAwesomeIcon icon={faTurnDown} />
            </button>
          </div>
        </div>
      </div>
    </aside>
  )
}
