var h=(w,m,s)=>new Promise((g,v)=>{var l=t=>{try{n(s.next(t))}catch(c){v(c)}},p=t=>{try{n(s.throw(t))}catch(c){v(c)}},n=t=>t.done?g(t.value):Promise.resolve(t.value).then(l,p);n((s=s.apply(w,m)).next())});import{d as j,cd as K,x as L,g as E,i as B,cj as H,b7 as M,o as x,a as V,u as o,f as i,j as I,w as f,k as C,b as a,F as O,t as F,e as Q,aH as q,K as z}from"./url.2e6a77c4.js";import{u as G}from"./replace-all.7cf5f327.js";import{_ as J}from"./index.f237f871.js";import{u as W}from"./use-shortcuts.21ce2d8e.js";import{_ as X}from"./index.051a5017.js";import{u as Y,_ as Z}from"./sidebar.983dd68b.js";import{_ as tt}from"./index.66d5d547.js";import{u as ot,E as et}from"./main.ea42d807.js";import{u as rt}from"./use-save-tip.04d9878f.js";import{K as it,L as at}from"./dev-mode.c4133b5a.js";import{g as mt}from"./i18n.48bd28ac.js";import{_ as st}from"./plugin-vue_export-helper.21dcd24c.js";import{E as pt}from"./index.fc90f5ad.js";import{E as lt,a as nt}from"./index.6855d037.js";import{E as ct}from"./index.a439b087.js";import{E as ut}from"./index.2bc50276.js";import"./basic.07a117d3.js";import"./index.a731e53b.js";import"./index.0d2684bf.js";import"./index.379e80ad.js";import"./windi.a5b0b048.js";import"./focus-trap.23f44899.js";import"./isNil.98bb3b88.js";import"./event.53b2ad83.js";import"./index.ff0264f3.js";import"./index.fcc3ea42.js";import"./error.7e8331f1.js";import"./event.776e7e11.js";import"./scroll.f51de5d8.js";import"./isEqual.f1ae9fb3.js";import"./_baseIsEqual.c0d7e77a.js";import"./debounce.61c67278.js";import"./toNumber.574be4f1.js";import"./index.064c7de9.js";import"./validator.e2869aba.js";import"./index.c4e9b529.js";import"./index.603c1365.js";import"./refs.4001ce17.js";import"./index.0f940e7f.js";import"./classCompletion.a22e38a6.js";import"./userWorker.b3a6730b.js";import"./editor.main.d2800f63.js";import"./preload-helper.13a99eb0.js";import"./guid.c1a40312.js";import"./vuedraggable.umd.5840ebc7.js";import"./cloneDeep.ff43c1f8.js";import"./_baseClone.adbc92f5.js";import"./index.cbddf8eb.js";import"./index.d4bbb472.js";import"./index.e3f90979.js";import"./validate.efc4a5c0.js";import"./index.d75a71d9.js";import"./index.aba77680.js";import"./index.6d45a031.js";import"./index.31f26a0f.js";import"./index.60af85f4.js";import"./style.19d0c187.js";import"./index.4fa52eca.js";import"./string.955a924b.js";import"./pickBy.a9d2c8dd.js";import"./_basePickBy.a6ffdd2d.js";import"./pick.db46e15f.js";import"./_createCompounder.591d8488.js";import"./isEmpty.dc565ae5.js";import"./last.e7aa49db.js";import"./index.50c16ae5.js";import"./index.aa73e4df.js";import"./index.ad581bad.js";import"./index.77edae39.js";import"./index.f5a869bb.js";import"./dropdown.0507a1c7.js";import"./confirm.e2c924ff.js";import"./logo-transparent.1566007e.js";import"./index.99c4f65d.js";import"./aria.75ec5909.js";import"./config.9fb52765.js";import"./dark.ddf8665a.js";import"./page.831d1a45.js";import"./use-copy-text.a346ed23.js";import"./index.c6df1b45.js";import"./toggleComment.5b29ca87.js";import"./index.695d0284.js";import"./icon-button.3313e42d.js";import"./index.8262f5ef.js";import"./index.232e741a.js";import"./index.af90dc36.js";import"./index.470f0e7e.js";import"./index.deca86b5.js";import"./alert.583ccfe6.js";import"./index.fafb14b3.js";/* empty css                                                               */import"./media-list.vue_vue_type_style_index_0_scoped_true_lang.118b18f5.js";/* empty css                                                          */import"./file.b0d4cc6e.js";import"./use-date.01b82ce0.js";import"./dayjs.min.0a66969b.js";/* empty css                                                          *//* empty css                                                                 */import"./use-file-upload.82732353.js";/* empty css                                                         */import"./image-editor.vue_vue_type_style_index_0_scoped_true_lang.f6d26366.js";import"./image-editor.846c8dc6.js";import"./main.esm.5190fb65.js";import"./index.f01d4ffd.js";const dt={key:0,class:"flex w-full h-full"},_t={class:"flex-1 pr-32 pl-80px flex flex-col min-h-400px min-w-600px"},ft={class:"pt-16"},vt={class:"flex-1 rounded-normal overflow-hidden shadow-s-10 mb-100px min-h-400px bg-fff"},ht={class:"bg-card dark:bg-[#333] shadow-s-10 w-400px h-full dark:text-fff/86"},xt={class:"pb-150px"},gt={class:"flex items-center px-24 py-12 leading-none"},bt={class:"flex-1 text-m"},wt=j({setup(w){const{t:m}=mt(),s=K(),g=L("id"),v=ot(),l=E(),p=rt((r,e)=>{if(!(r==="el"&&e))return e}),{init:n,model:t,save:c,preview:N,styles:u,scripts:d}=Y();n(g),B(()=>t.value,()=>{var r;z(()=>{p.init([t.value,u.value,d.value])}),l.value=(r=t.value)==null?void 0:r.urlPath});const P=E(),b=()=>h(this,null,function*(){var r,e;yield(r=P.value)==null?void 0:r.validate(),yield c(),et.success(m("common.saveSuccess")),p.init([t.value,u.value,d.value]),l.value=(e=t.value)==null?void 0:e.urlPath}),S=()=>{s.goBackOrTo(G({name:"pages"}))},R=()=>h(this,null,function*(){yield b(),S()});return H((r,e,_)=>h(this,null,function*(){r.name==="login"?_():yield p.check([t.value,u.value,d.value]).then(()=>{_()}).catch(()=>{_(!1)})})),B(()=>[t.value,u.value,d.value],()=>{p.changed([t.value,u.value,d.value])},{deep:!0}),W("save",b),it().loadAll(),at().loadAll(),(r,e)=>{const _=pt,T=lt,$=nt,A=ct,D=ut,U=M("hasPermission");return x(),V(O,null,[o(t)?(x(),V("div",dt,[i("div",_t,[i("div",ft,[o(t)?(x(),I($,{key:0},{default:f(()=>[a(T,{label:o(m)("common.pageName")},{default:f(()=>[a(_,{modelValue:o(t).name,"onUpdate:modelValue":e[0]||(e[0]=y=>o(t).name=y),title:o(t).name,class:"w-300px",disabled:"",onInput:e[1]||(e[1]=y=>{var k;return o(t).name=(k=o(t))==null?void 0:k.name.replace(/\s+/g,"")})},null,8,["modelValue","title"])]),_:1},8,["label"])]),_:1})):C("",!0)]),i("div",vt,[a(X,{content:o(N),"base-url":o(v).site.prUrl},null,8,["content","base-url"])])]),i("div",ht,[a(A,null,{default:f(()=>[i("div",xt,[i("div",gt,[i("p",bt,F(o(m)("common.setting")),1),a(tt)]),a(Z,{"old-url-path":l.value},null,8,["old-url-path"])])]),_:1})])])):C("",!0),a(J,{back:"",permission:{feature:"pages",action:"edit"},onCancel:S,onSave:b},{"extra-buttons":f(()=>[Q((x(),I(D,{round:"",type:"primary","data-cy":"save-and-return",onClick:R},{default:f(()=>[q(F(o(m)("common.saveAndReturn")),1)]),_:1})),[[U,{feature:"pages",action:"edit"}]])]),_:1})],64)}}});var be=st(wt,[["__scopeId","data-v-4e095ec5"]]);export{be as default};