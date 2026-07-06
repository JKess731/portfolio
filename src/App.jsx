import { Routes, Route } from 'react-router-dom'
import Nav from './components/Nav'
import Footer from './components/Footer'
import Home from './pages/Home'
import ProjectPage from './pages/ProjectPage'
import About from './pages/About'
import CodeSnippets from './pages/CodeSnippets'

export default function App() {
  return (
    <div className="flex min-h-screen flex-col">
      <Nav />
      <main className="flex-1">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/projects/:slug" element={<ProjectPage />} />
          <Route path="/about" element={<About />} />
          <Route path="/code-snippets" element={<CodeSnippets />} />
        </Routes>
      </main>
      <Footer />
    </div>
  )
}
