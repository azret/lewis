using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

static class App {
    class Entry {
        public string Declaration;
        public string Definition;
        public void correct() {
            if (Declaration.StartsWith("(")) {
                Declaration = Declaration.Remove(0, 1);
                int end = Declaration.IndexOf(")");
                if (end >= 0) {
                    Declaration = Declaration.Remove(end, 1);
                }
            }
        }
    }

    static void Main(string[] args) {
        try {
            string XML = File.ReadAllText(@"lewis.xml");

            List<Entry> DICT = compile(XML);

            foreach (var WORD in DICT) {
                WORD.correct();
            }

            Regex[] PATTERNS = Patterns();

            var UNK = new StringBuilder();

            Dictionary<char, StringBuilder> LINES = new Dictionary<char, StringBuilder>();

            foreach (var WORD in DICT) {
                Regex found = null;
                foreach (Regex REX in PATTERNS) {
                    if (REX.IsMatch(WORD.Declaration)) {
                        System.Diagnostics.Debug.Assert(found == null, "Duplicate regular expression pattern detected.");
                        found = REX;
                    }
                }
                if (found != null) {
                    char c = char.ToLowerInvariant(WORD.Declaration[0]);
                    if (c == '(') {
                        c = WORD.Declaration[1];
                    }
                    switch (c) {
                        case 'ā':
                            c = 'a';
                            break;
                        case 'ē':
                            c = 'e';
                            break;
                        case 'ō':
                            c = 'o';
                            break;
                        case 'ī':
                            c = 'i';
                            break;
                        case 'ū':
                            c = 'u';
                            break;
                    }
                    c = char.ToUpperInvariant(c);
                    if (!LINES.ContainsKey(c)) {
                        LINES[c] = new StringBuilder();
                    }
                    LINES[c].Append($"##### {WORD.Declaration}");
                    if (!string.IsNullOrWhiteSpace(WORD.Definition)) {
                        LINES[c].Append($"\r\n{WORD.Definition}");
                    }
                    LINES[c].Append($"\r\n");
                } else {
                    UNK.Append($"##### {WORD.Declaration}\r\n");
                }
            }

            foreach (var c in LINES) {
                if (c.Value != null) {
                    File.WriteAllText($"data//{c.Key}.md", c.Value.ToString());
                }
            }

            File.WriteAllText(@"lewis.md", UNK.ToString());

        } catch (Exception e) {
            Console.Error?.WriteLine(e);
        }

        Console.WriteLine("Done.");
    }

    static Regex[] Patterns() {
        Regex prōvocō = new Regex("^(.+)ō āvī, ātus, āre$", RegexOptions.None);
        Regex perpetuō = new Regex("^(.+)ō —, —, āre$", RegexOptions.None);
        Regex perplaceō = new Regex("^(.+)ō —, —, ēre$", RegexOptions.None);
        Regex circumsiliō = new Regex("^(.+)ō —, —, īre$", RegexOptions.None);
        Regex circumscindō = new Regex("^(.+)ō —, —, ere$", RegexOptions.None);
        Regex abstō = new Regex("^(.+)ō —, āre$", RegexOptions.None);
        Regex ablūdō = new Regex("^(.+)ō —, ere$", RegexOptions.None);
        Regex aberrō = new Regex("^(.+)ō āvī, —, āre", RegexOptions.None);
        Regex adv = new Regex("^(.+) (adv.)$", RegexOptions.None);
        Regex puella = new Regex("^(.+)a(,)? ae(,)? (f|m|n)$", RegexOptions.None);
        Regex abacus = new Regex("^(.+)(us|um) ī, (f|m|n)$", RegexOptions.None);
        Regex puerulus = new Regex("^(.+)(us|um) ī, (f|m|n) dim.$", RegexOptions.None);
        Regex volucris = new Regex("^(.+)is(,)? is(,)? (f|m|n)$", RegexOptions.None);
        Regex volūbilitās = new Regex("^(.+)ās(,)? ātis(,)? (f|m|n)$", RegexOptions.None);
        Regex ūsus = new Regex("^(.+)us(,)? ūs(,)? (f|m|n)$", RegexOptions.None);
        Regex pater = new Regex("^(.+)ter tris, (f|m|n)$", RegexOptions.None);
        Regex pedes = new Regex("^(.+)es(,)? itis(,)? (f|m|n)$", RegexOptions.None);
        Regex pēgma = new Regex("^(.+)es(,)? atis(,)? (f|m|n)$", RegexOptions.None);
        Regex abaliēnātiō = new Regex("^(.+)ātiō īnis, (f|m|n)$", RegexOptions.None);
        Regex volūtātiō = new Regex("^(.+)(i|r|b)ō ōnis, (f|m|n)$", RegexOptions.None);
        Regex pectus = new Regex("^(.+)tus oris, (f|m|n)$", RegexOptions.None);
        Regex perditor = new Regex("^(.+)(t|m|s|d|g|r|n|l|v|b)or ōris, (f|m|n)$", RegexOptions.None);
        Regex mōlēs = new Regex("^(.+)ēs is, (f|m|n)$", RegexOptions.None);
        Regex quiēs = new Regex("^(.+)ēs ētis, (f|m|n)$", RegexOptions.None);
        Regex mystēs = new Regex("^(.+)ēs ae, (f|m|n)$", RegexOptions.None);
        Regex Napaeae = new Regex("^(.+)ae ārum, (f|m|n)$", RegexOptions.None);
        Regex nātrīx = new Regex("^(.+)īx īcis, (f|m|n)$", RegexOptions.None);
        Regex fornix = new Regex("^(.+)ix icis, (f|m|n)$", RegexOptions.None);
        Regex nauseola = new Regex("^(.+)a ae, (f|m|n) dim.$", RegexOptions.None);
        Regex Lūceres = new Regex("^(.+)es(,)? um(,)? (f|m|n)$", RegexOptions.None);
        Regex quadrīgulae = new Regex("^(.+)ae ārum, (f|m|n) dim.$", RegexOptions.None);
        Regex mollitūdō = new Regex("^(.+)ūdō inis, (f|m|n)$", RegexOptions.None);
        Regex frāgmen = new Regex("^(.+)men inis, (f|m|n)$", RegexOptions.None);
        Regex vorāgō = new Regex("^(.+)gō inis, (f|m|n)$", RegexOptions.None);
        Regex pūlmō = new Regex("^(.+)(m|c|n)ō ōnis, (f|m|n)$", RegexOptions.None);
        Regex pȳramis = new Regex("^(.+)mis īdis, (f|m|n)$", RegexOptions.None);
        Regex quadrāns = new Regex("^(.+)āns antis, (f|m|n)$", RegexOptions.None);
        Regex rāmex = new Regex("^(.+)ex icis, (f|m|n)$", RegexOptions.None);
        Regex vectīgal = new Regex("^(.+)al ālis, (f|m|n)$", RegexOptions.None);
        Regex vīcēsimānī = new Regex("^(.+)ī ōrum, (f|m|n)$", RegexOptions.None);
        Regex Vīnālia = new Regex("^(.+)lia ium, (f|m|n)$", RegexOptions.None);
        Regex ānser = new Regex("^(.+)er eris, (f|m|n)$", RegexOptions.None);
        Regex antēs = new Regex("^(.+)ēs īum, (f|m|n)$", RegexOptions.None);
        Regex anthias = new Regex("^(.+)(as|ās)(,)? ae(,)? (f|m|n)$", RegexOptions.None);
        Regex verū = new Regex("^(.+)ū ūs, (f|m|n)$", RegexOptions.None);
        Regex virtūs = new Regex("^(.+)ūs ūtis, (f|m|n)$", RegexOptions.None);
        Regex animātus = new Regex("^(.+)us adj.$", RegexOptions.None);
        Regex dēditīcius = new Regex("^(.+)ius ī, adj.$", RegexOptions.None);
        Regex dēnicālis = new Regex("^(.+)lis ī, adj.$", RegexOptions.None);
        Regex volūbilis = new Regex("^(.+)(l|m|n|t)is e, adj.$", RegexOptions.None);
        Regex volēns = new Regex("^(.+)ēns entis, adj.$", RegexOptions.None);
        Regex vēsāniēns = new Regex("^(.+)ēns ntis, adj.$", RegexOptions.None);
        Regex viridāns = new Regex("^(.+)āns antis, adj.$", RegexOptions.None);
        Regex vigilāx = new Regex("^(.+)āx ācis, adj.$", RegexOptions.None);
        Regex dux = new Regex("^(.+)ux ucis, (f|m|n)$", RegexOptions.None);
        Regex rex = new Regex("^(.+)(ē|e)x (ē|e)gis, (f|m|n)$", RegexOptions.None);
        Regex vōx = new Regex("^(.+)ōx ōcis, (f|m|n)$", RegexOptions.None);
        Regex sandȳx = new Regex("^(.+)ȳx īcis, (f|m|n)$", RegexOptions.None);

        Regex[] PATTERNS = new Regex[] {
                circumscindō,
                circumsiliō,
                aberrō,
                sandȳx,
                abstō,
                ablūdō,
                prōvocō,
                perpetuō,
                Lūceres,
                perplaceō,
                rex,
                dux,
                vōx,
                vigilāx,
                vēsāniēns,
                viridāns,
                dēditīcius,
                dēnicālis,
                volūbilis,
                volēns,
                animātus,
                verū,
                virtūs,
                anthias,
                ānser,
                antēs,
                adv,
                puella,
                abacus,
                puerulus,
                volucris,
                volūbilitās,
                ūsus,
                pater,
                pedes,
                pēgma,
                abaliēnātiō,
                volūtātiō,
                pectus,
                perditor,
                mōlēs,
                quiēs,
                mystēs,
                Napaeae,
                nātrīx,
                fornix,
                nauseola,
                quadrīgulae,
                mollitūdō,
                frāgmen,
                vorāgō,
                pūlmō,
                pȳramis,
                quadrāns,
                rāmex,
                vectīgal,
                vīcēsimānī,
                Vīnālia,
            };
        return PATTERNS;
    }

    static List<Entry> compile(string XML) {
        Xml xml = new Xml(XML);
        xml.read();
        while (xml.Token == XmlTokenType.Text
                || xml.Token == XmlTokenType.SelfClosing
                            || xml.Token == XmlTokenType.ProcessingInstruction) {
            xml.read();
        }
        XmlNode doc = new XmlNode(XmlNodeType.Document, "#DOC", null);
        xml.parse(doc);
        List<Entry> DICT = new List<Entry>();
        traverse(DICT, doc);
        return DICT;
    }

    static XmlNode getElementByTagName(XmlNode node, string name) {
        for (int i = 0; node.Children != null && i < node.Children.Length; i++) {
            XmlNode child = node.Children[i];
            if (child.Is(name)) {
                return child;
            }
            var element = getElementByTagName(child, name);
            if (element != null) {
                return element;
            }
        }
        return null;
    }

    static XmlNode[] getElementsByTagName(XmlNode parent, params string[] names) {
        XmlNode[] elements = new XmlNode[0];
        void Enum(XmlNode node) {
            for (int i = 0; node.Children != null && i < node.Children.Length; i++) {
                XmlNode child = node.Children[i];
                foreach (string name in names) {
                    if (child.Is(name)) {
                        int len = elements.Length;
                        Array.Resize(ref elements, len + 1);
                        elements[len] = child;
                    }
                }
                Enum(child);
            }
        }
        Enum(parent);
        return elements;
    }

    static string purify(string data) {
        if (data == null) {
            data = "";
        }
        data = data.Replace("\r", 
            " ");
        data = data.Replace("\n",
            " ");
        data = data.Replace("\t",
            " ");
        while (data.IndexOf("  ") >= 0) {
            data = data.Replace(
                "  ",
                " ");
        }

        data = data.Replace(
            "—",
            " — ");
        while (data.IndexOf("( ") >= 0) {
            data = data.Replace(
             "( ",
             " (");
        }
        while (data.IndexOf("‘ ") >= 0) {
            data = data.Replace(
             "‘ ",
             "‘");
        }
        while (data.IndexOf(" ’") >= 0) {
            data = data.Replace(
             " ’",
             "’");
        }

        char[] breaks = { ')', '.', '?', ',', ':', ';', };
        int iters = 3;
        while (iters-- > 0) {
            foreach (char c in breaks) {
                while (data.IndexOf(" " + c.ToString()) >= 0) {
                    data = data.Replace(
                      " " + c.ToString(),
                     c.ToString());
                }
                data = data.Replace(
                  c.ToString(),
                 c.ToString() + " ");
            }
        }
        while (data.IndexOf(" .") >= 0) {
            data = data.Replace(
             " .",
             ".");
        }

        data = data.Replace(
            "*",
            "⋆");
        data = data.Replace(
            "#",
            "§");

        while (data.IndexOf(" )") >= 0) {
            data = data.Replace(
             " )",
             ")");
        }
        while (data.IndexOf("i. e.") >= 0) {
            data = data.Replace(
             "i. e.",
             "i. e.");
        }

        while (data.IndexOf("  ") >= 0) {
            data = data.Replace(
                "  ",
                " ");
        }

        return data.Trim();
    }

    static string abbr(string data) {
        data = data.Replace(
            " Iuv.",
            " ***Iuv.***");
        data = data.Replace(
            " O.",
            " ***Ov.***");
        data = data.Replace(
            " C.",
            " ***Cic.***");
        data = data.Replace(
            " Cs.",
            " ***Caes.***");
        data = data.Replace(
            " H.",
            " ***Hor.***");
        data = data.Replace(
            " N.",
            " ***Nep.***");
        data = data.Replace(
            " L.",
            " ***Liv.***");
        data = data.Replace(
            " Ta.",
            " ***Tac.***");
        data = data.Replace(
            " Iu.",
            " ***Iuv.***");
        data = data.Replace(
            " S.",
            " ***Sall.***");
        data = data.Replace(
            " V.",
            " ***Ver.***");
        data = data.Replace(
            " T.",
            " ***Ter.***");
        data = data.Replace(
            " Ph.",
            " ***Phaed.***");
        data = data.Replace(
            " Cu.",
            " ***Curt.***");
        data = data.Replace(
            " Pr.",
            " ***Prop.***");
        return data;
    }

    static bool ignore(string entry) {
        return entry == "A. a. as an abbreviation" ||
            entry == "ā";
    }

    static void innerText(XmlNode node, StringBuilder innerText, bool decorate) {
        for (int i = 0; node.Children != null && i < node.Children.Length; i++) {
            XmlNode child = node.Children[i];
            bool greek = false;
            if (node.Is("foreign")) {
                if (node.Data != null && node.Data.IndexOf("greek") >= 0) {
                    greek = true;
                }
            }
            if (child.Is(XmlNodeType.Text) && node.IsNot("etym")) {
                if (!greek && innerText != null) {
                    string text = purify(child.Data);
                    if (!string.IsNullOrWhiteSpace(text)) {
                        if (innerText.Length > 0) {
                            innerText.Append(" ");
                        }
                        if (decorate) {
                            if (node.Is("tr")) {
                                innerText.Append("");
                            }
                        }
                        innerText.Append(text);
                        if (decorate) {
                            if (node.Is("tr")) {
                                innerText.Append("");
                            }
                        }
                    }
                }
            } else {
                App.innerText(child, innerText, decorate);
            }
        }
    }

    static string innerText(XmlNode node, bool decorate) {
        StringBuilder innerText = new StringBuilder();
        App.innerText(
            node, 
            innerText,
            decorate);
        var text = abbr(purify(innerText.ToString())
            .TrimStart('\'', '.', ',')
            .TrimEnd('\'', ',').Trim());
        text = text.Replace("*,", "* ,");
        text = text.Replace("*:", "* :");
        text = text.Replace(":*", ": *");
        text = text.Replace(",*", ", *");
        return text;
    }

    static void traverse(List<Entry> DICT, XmlNode node) {
        for (int i = 0; node != null && node.Children != null && i < node.Children.Length; i++) {
            XmlNode entry = node.Children[i];
            if (entry.Is("entry")) {
                var orth = getElementByTagName(entry,  "orth");
                if (orth != null) {
                    StringBuilder decl = new StringBuilder();
                    decl.Append(innerText(orth, false).Replace("-", ""));
                    var gramGrp = getElementByTagName(entry, "gramGrp");
                    if (gramGrp != null) {
                        string text = innerText(gramGrp, false);
                        if (!string.IsNullOrWhiteSpace(text)) {
                            if (decl.Length > 0) decl.Append(" ");
                            decl.Append(text);
                        }
                    }
                    if (ignore(decl.ToString())) {
                        continue;
                    }
                    Entry word = new Entry() {
                        Declaration = decl.ToString()
                    };
                    var sense = getElementByTagName(entry, "sense");
                    if (sense != null) {
                        string text = innerText(sense, true);
                        if (string.IsNullOrWhiteSpace(text)) {
                        } else {
                            word.Definition = text;
                        }
                    }
                    DICT.Add(word);
                }
            } else {
                traverse(DICT, entry);
            }
        }
    }
}