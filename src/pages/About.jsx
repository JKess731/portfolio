import { useEffect } from 'react'
import { useLocation } from 'react-router-dom'
import ContactForm from '../components/ContactForm'

export default function About() {
  const { hash } = useLocation()

  useEffect(() => {
    if (!hash) return
    const el = document.querySelector(hash)
    el?.scrollIntoView({ behavior: 'smooth' })
  }, [hash])

  return (
    <div className="mx-auto max-w-5xl px-6 py-16">
      <h1 className="text-3xl font-semibold text-neutral-100">About</h1>
      <div className="mt-8 flex flex-col gap-8 sm:flex-row sm:items-start">
        <img
          src="/media/me.avif"
          alt="Jared Kessler"
          className="aspect-square w-64 shrink-0 rounded-lg object-cover sm:w-96"
        />
        <div className="flex flex-col gap-4">
          <p className="text-neutral-300">
            Hello! I'm Jared Kessler, a passionate programmer and game designer who
            thrives on learning quickly and tackling new challenges. I have a
            deep love for systems, and I'm always eager to push the limits of
            what's possible, whether it's crafting compelling stories or
            developing innovative gameplay mechanics. I believe every game has
            a distinct personality, and bringing that to life through my
            programming skills is something I find incredibly rewarding.
          </p>
          <p className="text-neutral-300">
            Currently, I am a software engineer at SHI International Corp. I work
            on the CRM team in Dyanmics 365. Working in D365 has allowed me to gain exposure
            into industry standards in agile development and my ability to learn on the fly
            in unfamiliar systems producing production ready code and components. I am able to
            gain hands-on experience working in C#/.NET, javascript, and power automate among many other
            systems within the enterprise.
          </p>
        </div>
      </div>

      <div className="mt-16">
        <h2 className="text-2xl font-semibold text-neutral-100">Background</h2>
        <div className="mt-4 flex max-w-2xl flex-col gap-4">
          <p className="text-neutral-300">
            At Indiana University, I focused on Game Development, specializing in programming, 
            production, and design. During my Junior year, I worked with classmates for 4 months
            to design a game concept and pitch it to industry professionals. This concept evolved into Slime Guy, which would be
            released on steam in December of 2025. My team and I have already been hard at a work on a brand new Untitled Fighting Game,
            to be released some time in the future.
          </p>
          <p className="text-neutral-300">
            I continue to work on games in my free time outside of work, to continue learning and growing my skills as I prepare
            to make the switch into the games industry.
          </p>
        </div>
      </div>

      <div id="contact" className="mt-16">
        <h2 className="text-2xl font-semibold text-neutral-100">Contact Me</h2>
        <p className="text-neutral-300">
          I'm always looking for exciting opportunites. Contact me here if youd like to chat!
        </p>
        <div className="mt-4">
          <ContactForm />
        </div>
      </div>
    </div>
  )
}
