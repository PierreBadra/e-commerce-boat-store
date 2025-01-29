/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,cshtml,html,js}"],
  theme: {
    extend: {
      gridTemplateColumns: {
        "responsive": "repeat(auto-fit, minmax(250px, 1fr))"
      }
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('flowbite/plugin')
  ],
}

