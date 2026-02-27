import json
import os

def escape_latex(text):
    if not text:
        return ""
    # Very basic LaTeX escape - can be expanded
    text = text.replace("&", r"\&")
    text = text.replace("%", r"\%")
    text = text.replace("$", r"\$")
    text = text.replace("#", r"\#")
    text = text.replace("_", r"\_")
    
    # Simple markdown to latex conversion for known elements
    text = text.replace("**Currently**", r"\textbf{Currently}")
    text = text.replace("**GITHUB**", r"\textbf{GITHUB}")
    text = text.replace("**railway simulator**", r"\textbf{railway simulator}")
    
    return text

def parse_markdown_links(text):
    import re
    # Convert [text](url) to \href{url}{text}
    pattern = r'\[(.*?)\]\((.*?)\)'
    return re.sub(pattern, r'\\href{\2}{\1}', text)

def generate_latex():
    with open('./cv-data.json', 'r', encoding='utf-8') as f:
        data = json.load(f)

    latex = r"""
\documentclass{article}
\usepackage{scimisc-cv}
\usepackage{fontawesome}
\usepackage{hyperref}
\usepackage{url}									

\title{AmatoCV}
\author{Amato Pierluigi}
\date{May 2020}

\cvname{Pierluigi Amato}
\cvpersonalinfo{
Cascina (PI), Tuscany, Italy\cvinfosep 
""" + data['Contact']['BirthDate'] + r""" \cvinfosep 
""" + data['Contact']['Phone'] + r"""\cvinfosep
""" + data['Contact']['Email'] + r"""\cvinfosep\href{""" + data['Contact']['LinkedIn'] + r"""}{\faLinkedinSquare}
}

\begin{document}

\makecvtitle 

\section{Summary}
"""
    # Summary
    for idx, p in enumerate(data['Summary']):
        p = parse_markdown_links(escape_latex(p))
        latex += p + ("\n\n" if idx < len(data['Summary'])-1 else "\n")

    # Jobs
    latex += r"""
\section{Jobs}
\begin{itemize}
"""
    for job in data['Jobs']:
        company_str = f", {job['Company']}" if job['Company'] else ""
        date_str = f", {job['Date']}" if job['Date'] else ""
        latex += f"\\item \\textbf{{{escape_latex(job['Title'])}{escape_latex(company_str)}{escape_latex(date_str)}}}\n"
        
        if job['Tasks']:
            latex += "\\begin{itemize}\n"
            for task in job['Tasks']:
                task_text = parse_markdown_links(escape_latex(task))
                latex += f"    \\item {task_text}\n"
            latex += "\\end{itemize}\n"

    latex += "\\end{itemize}\n\n"

    # Education
    latex += r""" 
\section{Education}

\begin{itemize}
"""
    for edu in data['Education']:
        date_str = f", {edu['Date']}" if edu['Date'] else ""
        latex += f"\\item {escape_latex(edu['Title'])}{escape_latex(date_str)}"
        if edu['Description']:
            latex += f", {parse_markdown_links(escape_latex(edu['Description']))}"
        latex += "\n\n"
    latex += "\\end{itemize}\n\n"

    # Technical Skills
    latex += r"""
\section{Technical Skills}

\begin{itemize}
"""
    for skill in data['TechnicalSkills']:
        latex += f"\\item \\textbf{{{escape_latex(skill['Category'])}:}}{escape_latex(skill['Details'])}\n"
    latex += "\\end{itemize}\n\n"

    # Projects
    latex += r""" 
\section{Projects}
\begin{itemize}
"""
    for proj in data['Projects']:
        latex += f"\\item \\textbf{{{escape_latex(proj['Title'])}}}"
        if proj['Description']:
            latex += "\n" + parse_markdown_links(escape_latex(proj['Description'])) + " \n"
            
        if proj['Tasks'] and len(proj['Tasks']) > 0:
            latex += "\\begin{itemize}\n"
            for task in proj['Tasks']:
                latex += f"\\item {escape_latex(task)}\n"
            latex += "\\end{itemize}\n\n"
        else:
            latex += "\n"

    latex += "\\end{itemize}\n\n"

    # University Projects (split into Subsections like original)
    thesis = next((p for p in data['UniversityProjects'] if "Thesis" in p['Category']), None)
    if thesis:
        latex += r"""\cvsubsection{Master Thesis}[SSSUP]
\begin{itemize}
"""
        latex += f"    \\item \\textbf{{{escape_latex(thesis['Title'])}}}\n    \\newline\n"
        latex += f"{parse_markdown_links(escape_latex(thesis['Description']))}\n"
        latex += "\\end{itemize}\n\n"

    latex += r"""\cvsubsection{Projects}[Pisa University, MS Mechatronics]
\begin{itemize}
"""
    for p in data['UniversityProjects']:
        if "Mechatronics" in p['Category']:
            latex += f"\\item \\textbf{{{escape_latex(p['Title'])}}}\n\n"
            latex += f"{parse_markdown_links(escape_latex(p['Description']))}\n\n"
    latex += "\\end{itemize}\n"

    bachelor = next((p for p in data['UniversityProjects'] if "Bachelor" in p['Category']), None)
    if bachelor:
        latex += r"""\cvsubsection{Bechelor's Degree}[Pisa University, Electronic Engineer]

\begin{itemize}
"""
        latex += f"\\item \\textbf{{{escape_latex(bachelor['Title'])}}}\n\n"
        latex += f"{escape_latex(bachelor['Description'])}\n\n"
        latex += "\\end{itemize}\n\n"

    # Other Skills
    latex += r""" \section{Other Skills}
\begin{description}[widest=Langauges]
"""
    for key, value in data['OtherSkills'].items():
        if key == "LATEX":
            latex += f"\\item[{key}] {parse_markdown_links(escape_latex(value).replace('LaTeX', r'\\LaTeX'))}\n"
        else:
            latex += f"\\item[{key}] {escape_latex(value)}\n"
            
    latex += r"""\end{description}


\end{document}
"""
    
    # Fix the Bechelor string issue due to AI autocomplete error above
    latex = latex.replace("Title'] подобные])", "Title'])}")

    with open('./Latex/main.tex', 'w', encoding='utf-8') as f:
        f.write(latex)
        
    print("LaTeX Generation completed successfully!")

if __name__ == "__main__":
    generate_latex()

