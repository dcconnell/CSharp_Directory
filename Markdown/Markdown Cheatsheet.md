# Headings
use #'s, number of #'s indicates heading type:
# Heading 1
## Heading 2
### Heading 3
#### Heading 4
##### Heading 5
###### Heading 6

```
# Heading 1
## Heading 2
### Heading 3
#### Heading 4
##### Heading 5
###### Heading 6
```

---
# Text options
+ Add to the front and back of text

*Italic text*
_Italic text_
```
*Italic text*
_Italic text_
```

**Bold text**
__Bold text__
```
**Bold text**
__Bold text__
```

***Bold italic text***
___Bold italic text___
```
***Bold italic text***
___Bold italic text___
```

~~Strikethrough text~~
```
~~Strikethrough text~~
```

==very important text==
```
==very important text==
```

`Inline code`
```
`Inline code`
```

\`\`\`Code Block\`\`\`
```
triple tilde for code block
```

denote code block starting triple tilde with code language for syntax highlighting, Ex.

```
```csharp
```
```csharp
public int multiplication(int x, int y)
{
	var x = 1;
	var y = 2;
	return x * y;
}
```

```
```json
```
```json
{  
   "allOrNone": false, // Please keep this as False  
   "records": [  
      {  
         "attributes": { "type": "Bill_Batch__c" },  
         "id": "aBOSu0000003L9ZOAU",  
         "State__c": "Run Complete",   
         "Request_ID__c": "xyz"   
      }  
   ]  
}
```

Blockquotes
> quote1
>> quote2
>>> quote3
```
> quote1
>> quote2
>>> quote3
```

---
# Lists

- option 1
- option 2
* option 3
+ option 4
```
- option 1
- option 2
* option 3
+ option 4
```

1. First item
2. Second item
	1. Subitem 1
	2. Subitem 2
3. Third item
```
1. First item
2. Second item
	1. Subitem 1
	2. Subitem 2
3. Third item
```

- [x] Completed task
- [ ] Incomplete task
- [ ] Another task
```
- [x] Completed task
- [ ] Incomplete task
- [ ] Another task
```

---
# Horizontal Rules

use three dashes ---

---
# Escape Characters

using a backslash \ the following characters can be escaped

\   Backslash
\`   Backtick
\*   Asterisk
\_   Underscore
\{}  Curly braces
\[]  Square brackets
\()  Parentheses
\#   Hash
\+   Plus
\-   Minus
\.   Period
\!   Exclamation mark

---
# Footnotes

Here is a footnote reference[^1].

[^1]: This is the content of the footnote.

---
# Supported HTML Tags

Markdown supports inline HTML:

<strong>Bold</strong>
```
<strong>Bold</strong>
```

<em>Italic</em>
```
<em>Italic</em>
```

<mark>Highlight</mark>
```
<mark>Highlight</mark>
```

<del>Delete</del>
```
<del>Delete</del>
```

<ins>Insert</ins>
```
<ins>Insert</ins>
```

H<sub>2</sub>O
```
H<sub>2</sub>O
```

X<sup>2</sup>
```
X<sup>2</sup>
```

---
# Math Formulas

Inline formula: $E = mc^2$
```
$E = mc^2$
```

Block formula:
$$
\sum_{i=1}^n a_i = 0
$$
```
$$
\sum_{i=1}^n a_i = 0
$$
```
---
# Links

[Title](https://www.example.com)
```
[Title](https://www.example.com)
```

---
# Image

```
![alt text](314785.jpg)
```

![alt text](314785.jpg)

---
