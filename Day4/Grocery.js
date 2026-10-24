
let productlist=new Map();
productlist.set("Apple",100)
productlist.set("Banana",50)
productlist.set("Orange",150)
productlist.set("Milk",230)
productlist.set("Meat",500)
productlist.set("Oil",350)
productlist.set("Water",270)
productlist.set("Sugar",370)

let List_of_Items=[];
function addproduct(){
    let product=document.getElementById("product").value;
    let quantity=document.getElementById("quantity").value;

    let price=productlist.get(product);

    List_of_Items.push({product, quantity, price});


}

function showproduct(){
    document.getElementById("output").innerHTML="";
    for(let i=0;i<List_of_Items.length;i++){
        document.getElementById("output").innerHTML+=List_of_Items[i].product + "   "+List_of_Items[i].quantity+"   "+List_of_Items[i].price+"<br>";
    }
    let total=0;
    for(let i=0;i<List_of_Items.length;i++){
        total+=List_of_Items[i].price*List_of_Items[i].quantity;
    }
    document.getElementById("bill").innerHTML="Total Bill:"+total;
}