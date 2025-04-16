import React from 'react'
import { createRoot } from 'react-dom/client'
import './styles.css'

function MemberSites() {
  return (
    <div className="container">
      <h1>Member Site Management</h1>
    </div>
  )
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <MemberSites />
  </React.StrictMode>
) 