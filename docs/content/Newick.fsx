(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
#r "../../bin/BioFSharp.dll"
#r "../../bin/BioFSharp.IO.dll"
#r "../../bin/FSharp.Care.dll"
#r "../../bin/FSharp.Care.IO.dll"

(** 

# Newick  


## Introducing the NewickFormat

The newick format is a simple, strictly symbolised format representing phylogenetic trees. It is the standard tree format used by the Clustal tool.  
In general, internal nodes (nodes with a least one descendant) are opened with `(` and closed with `)`. The childnodes within those internals are separated by `,`. After every node, there can be, but most not be information about its name and the distance from its parent. Name and distance are separated by `:`. Every full tree has a `;` at its end.  
These key characters are not allowed to be used within the given names or distances. This restriction allows a wide range of possible trees as can be seen by the following list of example trees:  

<br>
<button type="button" class="btn" data-toggle="collapse" data-target="#newickExample">Show/Hide example trees</button>
<div id="newickExample" class="collapse treeExamples ">
( taken from wikipedia )  

* (,,(,));  

   no nodes are named  

* (A,B,(C,D));  

   leaf nodes are named  

* (A,B,(C,D)E)F;  

   all nodes are named  

* (:0.1,:0.2,(:0.3,:0.4):0.5);  

   all but root node have a distance to parent  

* (:0.1,:0.2,(:0.3,:0.4):0.5):0.0;  

   all have a distance to parent  

* (A:0.1,B:0.2,(C:0.3,D:0.4):0.5);  

   distances and leaf names (popular)  

* (A:0.1,B:0.2,(C:0.3,D:0.4)E:0.5)F;  

   distances and all names  

* ((B:0.2,(C:0.3,D:0.4)E:0.5)F:0.1)A;  

   a tree rooted on a leaf node (rare)  

<button type="button" class="btn" data-toggle="collapse" data-target="#newickExample">Hide again</button>  

</div>
<br>

The parser integrated in BioFSharp tries to cover this wide range of possible input data by using a very generic tree representantion and allowing the user to parse it the way he wants by the usage of a mapping function as additional input. Of course having an idea about this tree implementation is necessary for working with the already parsed trees. It is therefore recommended to look into the `PhylTree`(API reference can be found [here](reference/biofsharp-phyltree.html)).

## The Newick Reader
To read a newick file, the function `ofFile` in `BioFSharp.IO.Newick` has to be used. It takes a mapping function of type `string -> 'Distance` and a path of type `string`. This means that names are always parsed as string but distances can be parsed at will. The usage of this is demonstrated here:  

Let's say we want to parse the following tree (which is btw output generated by clustal omega):

(  
(  
Mozarella:0.44256,  
Brie:-0.01399)  
:0.01161,  
Cheddar:-0.00714,  
(  
Emmentaler:0.00113,  
Gouda:-0.00113)  
:0.00714);  

After wrapping ones head about this symbol salad, one might find a few things about this tree:  
-Only the leaf nodes are named  
-Every node but the top node has a distance in form of a float number  
-All subtrees are binary  
<br>
Here is the same tree in the form of a cladogramm:  
![tree1](img/Tree.png)  
<br>
Now comes the parsing, as already mentioned, to bring the distance into a better form, we have to define a mapping function. In this case it's easy: we basically only need a function that transforms a string to a float *if possible*. Here it's important to mention that the parser interprets all nodes in the same way, as a functional programmer might expect. As not all nodes in the above tree have distances, that case has to be covered too: 

*)
open BioFSharp.IO

let fileDir = __SOURCE_DIRECTORY__ + "/data/"


//Maps string to float if possible. In the case it's not it just returns 0 instead
let floatMapping (distance: string) =
    try (float distance) with | _ -> 0.

//As path we set an examplefile included in biofsharp which consists of the tree shown above
let path = fileDir + "treeExample.txt"

Newick.ofFile floatMapping path

(**
The function returns the following result:  
<pre>
val it : BioFSharp.PhylTree.Node<float * string> =
  Branch
    ((0.0, ""),
     [Branch
        ((0.01161, ""),
         [Branch ((0.44256, "Mozarella"),[]); Branch ((-0.01399, "Brie"),[])]);
      Branch ((-0.00714, "Cheddar"),[]);
      Branch
        ((0.00714, ""),
         [Branch ((0.00113, "Emmentaler"),[]); Branch ((-0.00113, "Gouda"),[])])])
</pre>
*)

(**

As written above, having an idea about the `PhylTree` in BioFSharp makes understanding this result much easier. Its API reference can be found [here](reference/biofsharp-phyltree.html).  

## The Newick Reader  

Work in progress

*)